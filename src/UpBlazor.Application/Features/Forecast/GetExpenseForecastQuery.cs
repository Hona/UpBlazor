using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Schema;
using MediatR;
using UpBlazor.Application.Services;
using UpBlazor.Domain.Helpers;
using UpBlazor.Domain.Models;
using UpBlazor.Domain.Models.Enums;
using UpBlazor.Domain.Repositories;

namespace UpBlazor.Application.Features.Forecast;

public record GetExpenseForecastQuery(int TotalDays) : IRequest<IReadOnlyList<ForecastDto>>;

public class GetExpenseForecastQueryHandler : IRequestHandler<GetExpenseForecastQuery, IReadOnlyList<ForecastDto>>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly INormalizedAggregateRepository _normalizedAggregateRepository;
    private readonly IRecurringExpenseRepository _recurringExpenseRepository;
    private readonly IExpenseRepository _expenseRepository;
    private readonly IIncomeRepository _incomeRepository;
    private readonly IForecastService _forecastService;
    private readonly IMediator _mediator;

    public GetExpenseForecastQueryHandler(ICurrentUserService currentUserService, INormalizedAggregateRepository normalizedAggregateRepository, IRecurringExpenseRepository recurringExpenseRepository, IExpenseRepository expenseRepository, IIncomeRepository incomeRepository, IForecastService forecastService, IMediator mediator)
    {
        _currentUserService = currentUserService;
        _normalizedAggregateRepository = normalizedAggregateRepository;
        _recurringExpenseRepository = recurringExpenseRepository;
        _expenseRepository = expenseRepository;
        _incomeRepository = incomeRepository;
        _forecastService = forecastService;
        _mediator = mediator;
    }

    public async Task<IReadOnlyList<ForecastDto>> Handle(GetExpenseForecastQuery request, CancellationToken cancellationToken)
    {
        var userId = await _currentUserService.GetUserIdAsync(cancellationToken);

        var incomes = await _incomeRepository.GetAllByUserIdAsync(userId, cancellationToken);
        var expenses = await _expenseRepository.GetAllByUserIdAsync(userId, cancellationToken);
        var recurringExpenses = await _recurringExpenseRepository.GetAllByUserIdAsync(userId, cancellationToken);

        var rangeStart = DateTime.Now.Date;
        var rangeEnd = rangeStart.AddDays(request.TotalDays);

        var recurringExpenseCycleRanges = await _forecastService.GetRecurringExpenseCyclesInRangeAsync(rangeStart, rangeEnd, cancellationToken);
        var incomeCycleRanges = await _forecastService.GetIncomeCyclesInRangeAsync(rangeStart, rangeEnd, cancellationToken);

        var output = new Dictionary<DateOnly, List<ForecastDto>>();
        for (var i = 0; i < request.TotalDays; i++)
        {
            var currentDay = DateOnly.FromDateTime(rangeStart.AddDays(i));

            output[currentDay] = new List<ForecastDto>();

            // Order by smallest interval -> biggest so we add duplicate items for longer intervals
            foreach (var recurringExpense in recurringExpenses.OrderBy(x => x.Interval.ToTimeSpan(x.IntervalUnits)))
            {
                var cycleCollision = recurringExpenseCycleRanges[recurringExpense.Id].FirstOrDefault(x => x == currentDay);

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
                            accountName = recurringExpense.Name,
                            RecurringExpenseId = recurringExpense.Id
                        });
                        
                        continue;
                    }
                    
                    var lastValue = previousDayList
                        .FirstOrDefault(x => x.accountName == recurringExpense.Name);

                    if (lastValue is not null)
                    {
                        output[currentDay].Add(new ForecastDto
                        {
                            balance = lastValue.balance,
                            Index = i,
                            cycle = currentDay.ToString("dd/MM/yyyy"),
                            accountName = recurringExpense.Name,
                            RecurringExpenseId = recurringExpense.Id
                        });
                    }
                    
                    continue;
                }
                
                var expenseExact = recurringExpense.Money.Exact ?? throw new NotImplementedException(
                    "Currently cannot calculate percent based saver recurring expenses");
                
                output[currentDay].Add(new ForecastDto
                {
                    cycle = currentDay.ToString("dd/MM/yyyy"),
                    Index = i,
                    accountName = recurringExpense.Name,
                    balance = Math.Round(recurringExpenseCycleRanges[recurringExpense.Id].IndexOf(currentDay) * expenseExact, 2)
                });
            }
            
            foreach (var expense in expenses)
            {
                if (expense.FromIncomeId is not null)
                {
                    var income = incomeCycleRanges.Keys.First(x => x == expense.FromIncomeId.Value);
                    
                    var cycleCollision = incomeCycleRanges[income]
                        .FirstOrDefault(x => x == currentDay);

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
                                accountName = expense.Name,
                                ExpenseId = expense.Id
                            });
                            
                            continue;
                        }
                    
                        var lastValue = previousDayList
                            .FirstOrDefault(x => x.accountName == expense.Name);

                        if (lastValue is not null)
                        {
                            output[currentDay].Add(new ForecastDto
                            {
                                balance = lastValue.balance,
                                Index = i,
                                cycle = currentDay.ToString("dd/MM/yyyy"),
                                accountName = expense.Name,
                                ExpenseId = expense.Id
                            });
                        }
                    
                        continue;
                    }
                    
                    var expenseExact = expense.Money.Exact ?? expense.Money.Percent.Value * incomes.First(x => x.Id == income).ExactMoney;
                
                    output[currentDay].Add(new ForecastDto
                    {
                        cycle = currentDay.ToString("dd/MM/yyyy"),
                        Index = i,
                        accountName = expense.Name,
                        balance = Math.Round(incomeCycleRanges[income].IndexOf(currentDay) * expenseExact, 2),
                        ExpenseId = expense.Id
                    });
                }
                
                if (expense.FromSaverId is not null)
                {
                    if (currentDay < DateOnly.FromDateTime(expense.PaidByDate))
                    {
                        // Set as 0
                        output[currentDay].Add(new ForecastDto
                        {
                            cycle = currentDay.ToString("dd/MM/yyyy"),
                            Index = i,
                            accountName = expense.Name,
                            balance = 0,
                            ExpenseId = expense.Id,
                            SortPriority = -10
                        });
                    }
                    else
                    {
                        // The total will always be the cost
                        output[currentDay].Add(new ForecastDto
                        {
                            cycle = currentDay.ToString("dd/MM/yyyy"),
                            Index = i,
                            accountName = expense.Name,
                            balance = expense.Money.Exact.Value,
                            ExpenseId = expense.Id,
                            SortPriority = -10
                        });
                    }
                }
            }
        }
        
        return output
            .SelectMany(x => x.Value)
            .OrderByDescending(x => x.SortPriority)
            .ToList()
            .AsReadOnly();
    }
}