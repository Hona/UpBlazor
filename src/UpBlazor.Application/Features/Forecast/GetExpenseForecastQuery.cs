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

public record GetExpenseForecastQuery(int TotalDays) : IRequest<IReadOnlyList<ForecastDto>>;

public class GetExpenseForecastQueryHandler : IRequestHandler<GetExpenseForecastQuery, IReadOnlyList<ForecastDto>>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly INormalizedAggregateRepository _normalizedAggregateRepository;
    private readonly IRecurringExpenseRepository _recurringExpenseRepository;
    private readonly IExpenseRepository _expenseRepository;

    public GetExpenseForecastQueryHandler(ICurrentUserService currentUserService, INormalizedAggregateRepository normalizedAggregateRepository, IRecurringExpenseRepository recurringExpenseRepository, IExpenseRepository expenseRepository)
    {
        _currentUserService = currentUserService;
        _normalizedAggregateRepository = normalizedAggregateRepository;
        _recurringExpenseRepository = recurringExpenseRepository;
        _expenseRepository = expenseRepository;
    }

    public async Task<IReadOnlyList<ForecastDto>> Handle(GetExpenseForecastQuery request, CancellationToken cancellationToken)
    {
        var userId = await _currentUserService.GetUserIdAsync(cancellationToken);
        
        var normalizedAggregate = await _normalizedAggregateRepository.GetByUserIdAsync(userId, cancellationToken);
        var recurringExpenses = await _recurringExpenseRepository.GetAllByUserIdAsync(userId, cancellationToken);
        var expenses = await _expenseRepository.GetAllByUserIdAsync(userId, cancellationToken);

        var now = DateTime.Now.Date;

        var output = Enumerable.Range(0, request.TotalDays)
            .SelectMany(x =>
        {
            var output = new List<ForecastDto>(2 * request.TotalDays);

            output.AddRange(normalizedAggregate.RecurringExpenses.Select(normalizedRecurringExpense => new ForecastDto
            {
                balance = Math.Round(x * normalizedRecurringExpense.Amount, 2),
                cycle = now.AddDays(x).ToString("dd/MM/yyyy"),
                accountName = recurringExpenses.First(x => x.Id == normalizedRecurringExpense.RecurringExpenseId).Name,
                Index = x,
                RecurringExpenseId = normalizedRecurringExpense.RecurringExpenseId
            }));

            output.AddRange(expenses.Where(x => x.Money.Exact.HasValue).Select(expense => new ForecastDto
            {
                balance = expense.PaidByDate < now.AddDays(x) && expense.PaidByDate >= now ? Math.Round(expense.Money.Exact.Value, 2) : 0,
                cycle = now.AddDays(x).ToString("dd/MM/yyyy"),
                accountName = expense.Name,
                Index = x,
                ExpenseId = expense.Id
            }));
            
            output.AddRange(expenses.Where(x => x.Money.Percent.HasValue).Select(expense => new ForecastDto
            {
                balance = Math.Round(
                    x * (normalizedAggregate.Incomes.First(x => x.IncomeId == expense.FromIncomeId).Amount * expense.Money.Percent.Value), 2),
                cycle = now.AddDays(x).ToString("dd/MM/yyyy"),
                accountName = expense.Name,
                Index = x,
                ExpenseId = expense.Id
            }));

            return output;
        })
            .ToList()
            .AsReadOnly();
        return output;
    }
}