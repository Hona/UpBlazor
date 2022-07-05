using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Up.NET.Api.Accounts;
using UpBlazor.Application.Features.Expenses;
using UpBlazor.Application.Features.Incomes;
using UpBlazor.Application.Features.Planner;
using UpBlazor.Application.Features.RecurringExpenses;
using UpBlazor.Application.Features.Up;
using UpBlazor.Application.Services;
using UpBlazor.Core.Models;
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
    private readonly IForecastService _forecastService;

    public GetTotalForecastQueryHandler(INormalizedAggregateRepository normalizedAggregateRepository, ICurrentUserService currentUserService, IMediator mediator, ISavingsPlanRepository savingsPlanRepository, IExpenseRepository expenseRepository, IRecurringExpenseRepository recurringExpenseRepository, IForecastService forecastService)
    {
        _normalizedAggregateRepository = normalizedAggregateRepository;
        _currentUserService = currentUserService;
        _mediator = mediator;
        _savingsPlanRepository = savingsPlanRepository;
        _expenseRepository = expenseRepository;
        _recurringExpenseRepository = recurringExpenseRepository;
        _forecastService = forecastService;
    }

    public async Task<IReadOnlyList<ForecastDto>> Handle(GetTotalForecastQuery request, CancellationToken cancellationToken)
    {
        var accounts = await _mediator.Send(new GetUpAccountsQuery(), cancellationToken);
        var incomes = await _mediator.Send(new GetIncomesQuery(), cancellationToken);
        var recurringExpenses = await _mediator.Send(new GetRecurringExpensesQuery(), cancellationToken);

        var incomePlanners = new Dictionary<Guid, IncomePlannerDto>();
        foreach (var income in incomes)
        {
            var incomePlanner = await _mediator.Send(new GetIncomePlannerQuery(income, true), cancellationToken);

            incomePlanners[income.Id] = incomePlanner;
        }

        var rangeStart = DateTime.Now.Date;
        var rangeEnd = rangeStart.AddDays(request.TotalDays);

        var incomeCycles = await _forecastService.GetIncomeCyclesInRangeAsync(rangeStart, rangeEnd, cancellationToken);
        var recurringExpenseCycles = await _forecastService.GetRecurringExpenseCyclesInRangeAsync(rangeStart, rangeEnd, cancellationToken);
        var expenses = await _mediator.Send(new GetExpensesQuery(), cancellationToken);

        var output = new Dictionary<DateOnly, List<ForecastDto>>();
        for (var i = 0; i < request.TotalDays; i++)
        {
            var currentDay = DateOnly.FromDateTime(rangeStart.AddDays(i));

            output[currentDay] = new List<ForecastDto>();

            if (!output.TryGetValue(currentDay.AddDays(-1), out var yesterdaysBalances))
            {
                // No value - therefore we start with current balances
                yesterdaysBalances = accounts
                    .Select(x => new ForecastDto
                    {
                        UpAccountId = x.Id,
                        balance = x.Attributes.Balance.ValueInBaseUnits / 100M,
                        cycle = currentDay.ToString("dd/MM/yyyy"),
                        accountName = x.Attributes.DisplayName,
                        Index = i
                    })
                    .ToList();
            }

            var todaysIncomes = incomeCycles
                .Where(x => x.Value.Any(date => date == currentDay))
                .Select(x => x.Key)
                .ToList()
                .AsReadOnly();

            var todaysIncomeExpenses = new List<Expense>();
            foreach (var todaysIncome in todaysIncomes)
            {
                var todaysIncomeExpensesScoped = expenses
                    .Where(x => x.FromIncomeId.HasValue)
                    .Where(x => x.FromIncomeId.Value == todaysIncome)
                    .ToList()
                    .AsReadOnly();

                todaysIncomeExpenses.AddRange(todaysIncomeExpensesScoped);
            }

            var todaysRecurringExpenses = recurringExpenseCycles
                .Where(x => x.Value.Any(date => date == currentDay))
                .Select(x => x.Key)
                .ToList()
                .AsReadOnly();

            var todaysExpensesFromSavers = expenses
                .Where(x => !string.IsNullOrWhiteSpace(x.FromSaverId))
                .Where(x => DateOnly.FromDateTime(x.PaidByDate) == currentDay)
                .ToList()
                .AsReadOnly();

            var todaysIncomeExpensesTotalUnpaid = todaysIncomeExpenses
                .Select(x => x.Money.Exact ?? x.Money.Percent.Value * incomes.First(income => x.FromIncomeId == income.Id).ExactMoney)
                .Sum();
                
            
            foreach (var account in accounts)
            {
                var isTransactionalAccount = accounts[0].Id == account.Id;

                // Start with yesterday's balance
                var balance = yesterdaysBalances
                    .First(x => x.UpAccountId == account.Id)
                    .Clone();

                // Then - add any incomes
                foreach (var todaysIncome in todaysIncomes)
                {
                    var incomePlanner = incomePlanners[todaysIncome];

                    balance.balance += incomePlanner.FinalBudget
                        .First(x => x.Key == account.Id)
                        .Value;

                    // Income planner's final budget already has deducted the recurring expenses on a day average.
                    // Adding it back here, because we deduct recurring expenses exactly to the day further down
                    balance.balance += incomePlanner.ProRataExpenseSubTotals?
                        .Where(x => x.Amount.Exact is not null)
                        .Select(x => (decimal)x.Amount.Exact)
                        .Sum() ?? 0;
                    
                    // The incomePlanner.FinalBudget has already deducted income expenses
                    /*if (!isTransactionalAccount)
                    {
                        continue;
                    }
                    
                    var scopedIncomeExpenses = todaysIncomeExpenses
                        .Where(x => x.FromIncomeId.HasValue
                                    && x.FromIncomeId.Value == todaysIncome);

                    foreach (var scopedIncomeExpense in scopedIncomeExpenses)
                    {
                        balance.balance -= scopedIncomeExpense.Money.Exact
                                           ?? scopedIncomeExpense.Money.Percent.Value * incomes.First(x => x.Id == todaysIncome).ExactMoney;
                    }*/
                }
                
                if (todaysIncomeExpensesTotalUnpaid != 0)
                {
                    if (todaysIncomeExpensesTotalUnpaid > balance.balance)
                    {
                        todaysIncomeExpensesTotalUnpaid -= balance.balance;
                        balance.balance = 0;
                    }
                    else
                    {
                        balance.balance -= todaysIncomeExpensesTotalUnpaid;
                        todaysIncomeExpensesTotalUnpaid = 0;
                    }
                }

                // Subtract any recurring expenses
                foreach (var todaysRecurringExpense in todaysRecurringExpenses
                             .Select(x => recurringExpenses.First(recurringExpense => recurringExpense.Id == x))
                             .Where(x => x.SaverId == account.Id))
                {
                    balance.balance -= todaysRecurringExpense.Money.Exact ??
                                       throw new NotImplementedException("Cannot do % off saver yet");
                }

                // Subtract any one off expenses
                foreach (var todaysExpenseFromSaver in todaysExpensesFromSavers
                             .Where(x => x.FromSaverId == account.Id))
                {
                    balance.balance -= todaysExpenseFromSaver.Money.Exact ??
                                       throw new NotImplementedException("Cannot do % off saver yet");
                }

                output[currentDay].Add(new ForecastDto
                {
                    balance = Math.Round(balance.balance, 2),
                    cycle = currentDay.ToString("dd/MM/yyyy"),
                    accountName = account.Attributes.DisplayName,
                    Index = i,
                    UpAccountId = account.Id
                });
            }
        }

        return output
            .SelectMany(x => x.Value)
            .ToList()
            .AsReadOnly();

        /*var userId = await _currentUserService.GetUserIdAsync(cancellationToken);

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
        return output;*/
    }
}