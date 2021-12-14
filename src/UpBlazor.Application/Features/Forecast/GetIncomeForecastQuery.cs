using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UpBlazor.Application.Services;
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
        
        var normalizedAggregate = await _normalizedAggregateRepository.GetByUserIdAsync(userId, cancellationToken);
        var incomes = await _incomeRepository.GetAllByUserIdAsync(userId, cancellationToken);
        
        var now = DateTime.Now.Date;

        var output = Enumerable.Range(0, request.TotalDays)
            .SelectMany(x => normalizedAggregate.Incomes
                .Select(normalizedIncome => new ForecastDto
                {
                    balance = Math.Round(x * normalizedIncome.Amount, 2),
                    cycle = now.AddDays(x).ToString("dd/MM/yyyy"),
                    accountName = incomes.First(x => x.Id == normalizedIncome.IncomeId).Name,
                    Index = x
                })
                .ToList())
            .ToList()
            .AsReadOnly();
        return output;
    }
}