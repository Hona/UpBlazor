using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UpBlazor.Application.Features.Incomes;
using UpBlazor.Application.Features.RecurringExpenses;
using UpBlazor.Core.Helpers;

namespace UpBlazor.Application.Services;

public class ForecastService : IForecastService
{
    private readonly IMediator _mediator;

    public ForecastService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<Dictionary<Guid, List<DateOnly>>> GetRecurringExpenseCyclesInRangeAsync(DateTime rangeStart, DateTime rangeEnd, CancellationToken cancellationToken = default)
    {
        var recurringExpenses = await _mediator.Send(new GetRecurringExpensesQuery(), cancellationToken);
        
        var recurringExpenseCycleRanges = new Dictionary<Guid, List<DateOnly>>();

        foreach (var recurringExpense in recurringExpenses)
        {
            recurringExpenseCycleRanges[recurringExpense.Id] = recurringExpense.StartDate.Date.GetAllCyclesInRange(rangeStart,
                rangeEnd, recurringExpense.Interval, recurringExpense.IntervalUnits);
        }

        return recurringExpenseCycleRanges;
    }
    
    public async Task<Dictionary<Guid, List<DateOnly>>> GetIncomeCyclesInRangeAsync(DateTime rangeStart, DateTime rangeEnd, CancellationToken cancellationToken = default)
    {
        var incomes = await _mediator.Send(new GetIncomesQuery(), cancellationToken);
        
        var incomeCycleRanges = new Dictionary<Guid, List<DateOnly>>();

        foreach (var income in incomes)
        {
            incomeCycleRanges[income.Id] = income.StartDate.Date.GetAllCyclesInRange(rangeStart,
                rangeEnd, income.Interval, income.IntervalUnits);
        }

        return incomeCycleRanges;
    }
}