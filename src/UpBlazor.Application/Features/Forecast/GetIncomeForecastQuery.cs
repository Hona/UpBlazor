using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UpBlazor.Application.Services;
using UpBlazor.Core.Helpers;
using UpBlazor.Core.Models;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Application.Features.Forecast;

public record GetIncomeForecastQuery(int TotalDays) : IRequest<IReadOnlyList<ForecastDto>>;

public class GetIncomeForecastQueryHandler : IRequestHandler<GetIncomeForecastQuery, IReadOnlyList<ForecastDto>>
{
    private readonly INormalizedAggregateRepository _normalizedAggregateRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IIncomeRepository _incomeRepository;

    public GetIncomeForecastQueryHandler(INormalizedAggregateRepository normalizedAggregateRepository, IIncomeRepository incomeRepository, ICurrentUserService currentUserService)
    {
        _normalizedAggregateRepository = normalizedAggregateRepository;
        _incomeRepository = incomeRepository;
        _currentUserService = currentUserService;
    }

    public async Task<IReadOnlyList<ForecastDto>> Handle(GetIncomeForecastQuery request, CancellationToken cancellationToken)
    {
        var userId = await _currentUserService.GetUserIdAsync(cancellationToken);

        var incomes = await _incomeRepository.GetAllByUserIdAsync(userId, cancellationToken);

        var rangeStart = DateTime.Now.Date;
        var rangeEnd = rangeStart.AddDays(request.TotalDays);

        var incomeCycleRanges = new Dictionary<Income, List<DateOnly>>();

        foreach (var income in incomes)
        {
            incomeCycleRanges[income] = income.StartDate.GetAllCyclesInRange(rangeStart, rangeEnd, income.Interval, income.IntervalUnits);
        }

        var output = new Dictionary<DateOnly, List<ForecastDto>>();
        for (var i = 0; i < request.TotalDays; i++)
        {
            var currentDay = DateOnly.FromDateTime(rangeStart.AddDays(i));

            output[currentDay] = new List<ForecastDto>();
            
            // Order by smallest interval -> biggest so we add duplicate items for longer intervals
            foreach (var income in incomes.OrderBy(x => x.Interval.ToTimeSpan(x.IntervalUnits)))
            {
                var cycleCollision = incomeCycleRanges[income].FirstOrDefault(x => x == currentDay);

                if (cycleCollision == default)
                {
                    // Add the same as the last day - no change because the graph is at a smaller unit than the cycle

                    if (!output.TryGetValue(currentDay.AddDays(-1), out var previousDayList))
                    {
                        output[currentDay].Add(new ForecastDto
                        {
                            balance = 0,
                            Index = i,
                            cycle = currentDay.ToString("dd/MM/yyyy"),
                            accountName = income.Name
                        });
                        
                        continue;
                    }
                    
                    var lastValue = previousDayList
                        .FirstOrDefault(x => x.accountName == income.Name);

                    if (lastValue is not null)
                    {
                        output[currentDay].Add(new ForecastDto
                        {
                            balance = lastValue.balance,
                            Index = i,
                            cycle = currentDay.ToString("dd/MM/yyyy"),
                            accountName = income.Name
                        });
                    }
                    
                    continue;
                }
                
                output[currentDay].Add(new ForecastDto
                {
                    cycle = currentDay.ToString("dd/MM/yyyy"),
                    Index = i,
                    accountName = income.Name,
                    balance = Math.Round((incomeCycleRanges[income].IndexOf(currentDay) + 1) * income.ExactMoney, 2)
                });
            }
        }

        return output
            .SelectMany(x => x.Value)
            .ToList()
            .AsReadOnly();
    }
}