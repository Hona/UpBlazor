using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Up.NET.Api.Accounts;
using UpBlazor.Application.Features.Up;
using UpBlazor.Application.Services;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Application.Features.Forecast;

public record GetTotalForecastQuery(int TotalDays, IReadOnlyList<ForecastDto> ExpensesForecastData = null) : IRequest<IReadOnlyList<ForecastDto>>;

public class GetTotalForecastQueryHandler : IRequestHandler<GetTotalForecastQuery, IReadOnlyList<ForecastDto>>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly INormalizedAggregateRepository _normalizedAggregateRepository;
    private readonly IMediator _mediator;
    private readonly ISavingsPlanRepository _savingsPlanRepository;
    private readonly IExpenseRepository _expenseRepository;
    private readonly IRecurringExpenseRepository _recurringExpenseRepository;

    public GetTotalForecastQueryHandler(INormalizedAggregateRepository normalizedAggregateRepository, ICurrentUserService currentUserService, IMediator mediator, ISavingsPlanRepository savingsPlanRepository, IExpenseRepository expenseRepository, IRecurringExpenseRepository recurringExpenseRepository)
    {
        _normalizedAggregateRepository = normalizedAggregateRepository;
        _currentUserService = currentUserService;
        _mediator = mediator;
        _savingsPlanRepository = savingsPlanRepository;
        _expenseRepository = expenseRepository;
        _recurringExpenseRepository = recurringExpenseRepository;
    }

    public async Task<IReadOnlyList<ForecastDto>> Handle(GetTotalForecastQuery request, CancellationToken cancellationToken)
    {
        var userId = await _currentUserService.GetUserIdAsync(cancellationToken);

        var normalizedAggregate = await _normalizedAggregateRepository.GetByUserIdAsync(userId, cancellationToken);
        var savingsPlans = await _savingsPlanRepository.GetAllByUserIdAsync(userId, cancellationToken);
        var expenses = await _expenseRepository.GetAllByUserIdAsync(userId, cancellationToken);
        var recurringExpenses = await _recurringExpenseRepository.GetAllByUserIdAsync(userId, cancellationToken);
        
        var totalSavingsPlanAmount = normalizedAggregate
            .SavingsPlans
            .Sum(x => x.Amount);
        var totalIncomeAmounts = normalizedAggregate
            .Incomes
            .Sum(x => x.Amount);
        var unbudgetedAmount = totalIncomeAmounts - totalSavingsPlanAmount;

        var accounts = await _mediator.Send(new GetUpAccountsQuery(), cancellationToken);
        
        var expensesForecastData = request.ExpensesForecastData 
                                   ?? await _mediator.Send(new GetExpenseForecastQuery(request.TotalDays), cancellationToken);

        var now = DateTime.Now.Date;


        var output = Enumerable.Range(0, request.TotalDays)
            .SelectMany(x =>
        {
            var unbudgetedAmountUsed = false;

            return accounts.Select(account =>
            {
                var savingsPlanIdsFiltered = savingsPlans
                    .Where(x => x.SaverId == account.Id)
                    .Select(x => x.Id)
                    .ToList();

                var totalNormalizedAccountAggregate = normalizedAggregate.SavingsPlans
                    .Where(x => savingsPlanIdsFiltered.Contains(x.SavingsPlanId))
                    .Sum(x => x.Amount);

                if (!unbudgetedAmountUsed && account.Attributes.AccountType == AccountType.Transactional)
                {
                    totalNormalizedAccountAggregate += unbudgetedAmount;
                    unbudgetedAmountUsed = true;
                }

                var predictedBalance = totalNormalizedAccountAggregate * x;

                predictedBalance += account.Attributes.Balance.ValueInBaseUnits / 100M;

                predictedBalance -= expensesForecastData
                    .Where(expensesVm => expensesVm.Index == x)
                    .Where(expensesVm =>
                    {
                        if (expensesVm.ExpenseId.HasValue)
                        {
                            var matchingExpense = expenses.First(x => x.Id == expensesVm.ExpenseId.Value);

                            if (matchingExpense.SaverId == account.Id)
                            {
                                return true;
                            }
                        }

                        if (expensesVm.RecurringExpenseId.HasValue)
                        {
                            var matchingRecurringExpense = recurringExpenses.First(x => x.Id == expensesVm.RecurringExpenseId.Value);

                            if (matchingRecurringExpense.SaverId == account.Id)
                            {
                                return true;
                            }
                        }

                        return false;
                    })
                    .Sum(x => x.balance);

                return new ForecastDto
                {
                    balance = Math.Round(predictedBalance, 2),
                    cycle = now.AddDays(x).ToString("dd/MM/yyyy"),
                    accountName = account.Attributes.DisplayName
                };
            });
        })
            .ToList()
            .AsReadOnly();
        return output;
    }
}