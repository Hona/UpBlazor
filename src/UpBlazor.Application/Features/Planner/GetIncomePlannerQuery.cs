using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Up.NET.Api.Accounts;
using UpBlazor.Application.Features.Up;
using UpBlazor.Application.Services;
using UpBlazor.Core.Exceptions;
using UpBlazor.Core.Models;
using UpBlazor.Core.Models.Enums;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Application.Features.Planner;

public record GetIncomePlannerQuery(Guid IncomeId, bool OnlyUseSavingsPlans = false) : IRequest<IncomePlannerDto>;

public class GetIncomePlannerQueryHandler : IRequestHandler<GetIncomePlannerQuery, IncomePlannerDto>
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly INormalizedAggregateRepository _normalizedAggregateRepository;
    private readonly IRecurringExpenseRepository _recurringExpenseRepository;
    private readonly ISavingsPlanRepository _savingsPlanRepository;
    private readonly IIncomeRepository _incomeRepository;
    private readonly IMediator _mediator;

    public GetIncomePlannerQueryHandler(IExpenseRepository expenseRepository, ICurrentUserService currentUserService, INormalizedAggregateRepository normalizedAggregateRepository, IRecurringExpenseRepository recurringExpenseRepository, ISavingsPlanRepository savingsPlanRepository, IMediator mediator, IIncomeRepository incomeRepository)
    {
        _expenseRepository = expenseRepository;
        _currentUserService = currentUserService;
        _normalizedAggregateRepository = normalizedAggregateRepository;
        _recurringExpenseRepository = recurringExpenseRepository;
        _savingsPlanRepository = savingsPlanRepository;
        _mediator = mediator;
        _incomeRepository = incomeRepository;
    }

    public async Task<IncomePlannerDto> Handle(GetIncomePlannerQuery request, CancellationToken cancellationToken)
    {
        // TODO: Should this be 4 separate queries?
        var userId = await _currentUserService.GetUserIdAsync(cancellationToken);

        var incomes = await _incomeRepository.GetAllByUserIdAsync(userId, cancellationToken);

        var income = incomes.FirstOrDefault(x => x.Id == request.IncomeId);
        if (income is null)
        {
            throw new BadRequestException("Income not found");
        }
        
        var expenses = await _expenseRepository.GetAllByUserIdAsync(userId, cancellationToken);
        var normalizedAggregate = await _normalizedAggregateRepository.GetByUserIdAsync(userId, cancellationToken);

        if (normalizedAggregate is null)
        {
            throw new ArgumentNullException(nameof(normalizedAggregate));
        }
        
        var recurringExpenses = await _recurringExpenseRepository.GetAllByUserIdAsync(userId, cancellationToken);
        var savingsPlans = await _savingsPlanRepository.GetAllByIncomeIdAsync(income.Id, cancellationToken);

        var accounts = await _mediator.Send(new GetUpAccountsQuery(), cancellationToken);

        var output = new IncomePlannerDto
        {
            UnbudgetedMoney = income.ExactMoney
        };

        if (!request.OnlyUseSavingsPlans)
        {
            GetIncomeExpenseSubTotals(income, output, expenses);
            GetProRataExpenseSubTotals(income, output, normalizedAggregate, recurringExpenses);
        }

        GetExactSubTotals(output, savingsPlans);
        GetPercentSubTotals(income, output, savingsPlans);

        GetFinalBudget(income, output, accounts);

        RoundAllValues(output);

        return output;
    }

    private void RoundAllValues(IncomePlannerDto output)
    {
        // Round final budget
        var finalBudgetKeys = output.FinalBudget.Keys.ToList();
        foreach (var finalBudgetKey in finalBudgetKeys)
        {
            output.FinalBudget[finalBudgetKey] = Math.Round(output.FinalBudget[finalBudgetKey], 2);
        }

        // Round unbudgeted money
        output.UnbudgetedMoney = Math.Round(output.UnbudgetedMoney, 2);


        // Round all running totals
        void RoundAllSavingsPlanRunningTotal(IEnumerable<SavingsPlanRunningTotal> list)
        {
            if (list is null)
            {
                return;
            }

            foreach (var model in list)
            {
                model.RunningTotal = Math.Round(model.RunningTotal, 2);
            }
        }

        RoundAllSavingsPlanRunningTotal(output.ExactSavingsPlanSubTotals);
        RoundAllSavingsPlanRunningTotal(output.PercentSavingsPlanSubTotals);
        RoundAllSavingsPlanRunningTotal(output.IncomeExpenseSubTotals);
        RoundAllSavingsPlanRunningTotal(output.ProRataExpenseSubTotals);
    }

    private static void GetFinalBudget(Income income, IncomePlannerDto output, IReadOnlyList<AccountResource> accounts)
    {
        output.FinalBudget = new Dictionary<string, decimal>();

        var unbudgetedMoney = output.UnbudgetedMoney;

        foreach (var account in accounts)
        {
            var total = 0M;

            if (account.Attributes.AccountType == AccountType.Transactional)
            {
                total += unbudgetedMoney;
                unbudgetedMoney = 0;
            }

            foreach (var exactSaving in output.ExactSavingsPlanSubTotals.Where(x => x.SaverId == account.Id))
            {
                total += exactSaving.Amount.Exact.Value;
            }

            foreach (var percentSaving in output.PercentSavingsPlanSubTotals.Where(x => x.SaverId == account.Id))
            {
                total += income.ExactMoney * percentSaving.Amount.Percent.Value;
            }

            output.FinalBudget[account.Id] = total;
        }
    }

    private static void GetPercentSubTotals(Income income, IncomePlannerDto output,
        IReadOnlyList<SavingsPlan> savingsPlans)
    {
        output.PercentSavingsPlanSubTotals = new List<SavingsPlanRunningTotal>();

        foreach (var savingsPlan in savingsPlans.Where(x => x.Amount.Percent.HasValue))
        {
            output.UnbudgetedMoney -= income.ExactMoney * savingsPlan.Amount.Percent.Value;

            output.PercentSavingsPlanSubTotals.Add(new SavingsPlanRunningTotal(savingsPlan, output.UnbudgetedMoney));
        }
    }

    private static void GetExactSubTotals(IncomePlannerDto output, IReadOnlyList<SavingsPlan> savingsPlans)
    {
        output.ExactSavingsPlanSubTotals = new List<SavingsPlanRunningTotal>();

        foreach (var savingsPlan in savingsPlans.Where(x => x.Amount.Exact.HasValue))
        {
            output.UnbudgetedMoney -= savingsPlan.Amount.Exact.Value;

            output.ExactSavingsPlanSubTotals.Add(new SavingsPlanRunningTotal(savingsPlan, output.UnbudgetedMoney));
        }
    }

    private static void GetProRataExpenseSubTotals(Income income, IncomePlannerDto output,
        NormalizedAggregate normalizedAggregate, IReadOnlyList<RecurringExpense> recurringExpenses)
    {
        output.ProRataExpenseSubTotals = new List<SavingsPlanRunningTotal>();

        foreach (var recurringExpense in normalizedAggregate.RecurringExpenses)
        {
            var originalRecurringExpense = recurringExpenses.First(x => x.Id == recurringExpense.RecurringExpenseId);

            var proRataAmount = recurringExpense.Amount * income.IntervalUnits * income.Interval switch
            {
                Interval.Days => 1,
                Interval.Weeks => 7,
                Interval.Fortnights => 14,
                _ => throw new ArgumentOutOfRangeException()
            };

            output.UnbudgetedMoney -= proRataAmount;

            output.ProRataExpenseSubTotals.Add(new SavingsPlanRunningTotal(originalRecurringExpense, proRataAmount,
                output.UnbudgetedMoney));
        }
    }

    private static void GetIncomeExpenseSubTotals(Income income, IncomePlannerDto output,
        IReadOnlyList<Expense> expenses)
    {
        output.IncomeExpenseSubTotals = new List<SavingsPlanRunningTotal>();

        foreach (var incomeExpense in expenses.Where(x => x.FromIncomeId == income.Id))
        {
            if (incomeExpense.Money.Exact.HasValue)
            {
                output.UnbudgetedMoney -= incomeExpense.Money.Exact.Value;

                output.IncomeExpenseSubTotals.Add(new SavingsPlanRunningTotal(incomeExpense, output.UnbudgetedMoney));
            }

            if (incomeExpense.Money.Percent.HasValue)
            {
                output.UnbudgetedMoney -= income.ExactMoney * incomeExpense.Money.Percent.Value;

                output.IncomeExpenseSubTotals.Add(new SavingsPlanRunningTotal(incomeExpense, output.UnbudgetedMoney));
            }
        }
    }
}