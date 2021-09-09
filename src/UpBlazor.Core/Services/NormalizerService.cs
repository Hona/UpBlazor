using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UpBlazor.Core.Helpers;
using UpBlazor.Core.Models;
using UpBlazor.Core.Models.Normalized;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Core.Services
{
    public class NormalizerService : INormalizerService
    {
        private readonly IIncomeRepository _incomeRepository;
        private readonly IIncomeGoalRepository _incomeGoalRepository;
        private readonly IRecurringExpenseRepository _recurringExpenseRepository;

        private readonly INormalizedAggregateRepository _normalizedAggregateRepository;

        public NormalizerService(IIncomeRepository incomeRepository, IIncomeGoalRepository incomeGoalRepository, IRecurringExpenseRepository recurringExpenseRepository, INormalizedAggregateRepository normalizedAggregateRepository)
        {
            _incomeRepository = incomeRepository;
            _incomeGoalRepository = incomeGoalRepository;
            _recurringExpenseRepository = recurringExpenseRepository;
            _normalizedAggregateRepository = normalizedAggregateRepository;
        }

        public async Task UpdateUserAsync(string userId)
        {
            var incomesTask = _incomeRepository.GetAllByUserIdAsync(userId);
            var incomeGoalsTask = _incomeGoalRepository.GetAllByUserIdAsync(userId);
            var recurringExpensesTask = _recurringExpenseRepository.GetAllByUserIdAsync(userId);

            var incomes = await incomesTask;
            var incomeGoals = await incomeGoalsTask;
            var recurringExpenses = await recurringExpensesTask;

            var aggregate = new NormalizedAggregate(userId);
            
            NormalizeIncomes(aggregate, incomes);
            NormalizeIncomeGoals(aggregate, incomeGoals, incomes);
            NormalizeRecurringExpenses(aggregate, recurringExpenses, incomes);

            await _normalizedAggregateRepository.AddOrUpdateAsync(aggregate);
        }

        private static void NormalizeRecurringExpenses(NormalizedAggregate output, IEnumerable<RecurringExpense> recurringExpenses, IReadOnlyList<Income> incomes)
        {
            foreach (var recurringExpense in recurringExpenses)
            {
                var interval = recurringExpense.Interval.ToTimeSpan(recurringExpense.IntervalUnits);

                var totalDays = (decimal)interval.TotalDays;

                var amount = 0M;

                if (recurringExpense.Money.Exact.HasValue)
                {
                    amount = recurringExpense.Money.Exact.Value / totalDays;
                }

                if (recurringExpense.Money.Percent.HasValue)
                {
                    if (recurringExpense.FromIncomeId.HasValue)
                    {
                        var income = incomes.First(x => x.Id == recurringExpense.FromIncomeId);

                        amount = income.ExactMoney * recurringExpense.Money.Percent.Value / totalDays;
                    }

                    if (recurringExpense.FromSaverId != null)
                    {
                        throw new NotImplementedException("Currently cannot normalize percent based saver recurring expenses");
                    }
                }

                output.RecurringExpenses.Add(new NormalizedRecurringExpense
                {
                    RecurringExpenseId = recurringExpense.Id,
                    Amount = amount
                });
            }
        }

        private static void NormalizeIncomeGoals(NormalizedAggregate output, IEnumerable<IncomeGoal> incomeGoals, IReadOnlyList<Income> incomes)
        {
            foreach (var incomeGoal in incomeGoals)
            {
                var income = incomes.First(x => x.Id == incomeGoal.IncomeId);
                
                var interval = income.Interval.ToTimeSpan(income.IntervalUnits);

                var totalDays = (decimal)interval.TotalDays;
                
                output.IncomeGoals.Add(new NormalizedIncomeGoal
                {
                    IncomeGoalId = incomeGoal.Id,
                    Amount = income.ExactMoney / totalDays
                });
            }
        }

        private static void NormalizeIncomes(NormalizedAggregate aggregate, IEnumerable<Income> incomes)
        {
            foreach (var income in incomes)
            {
                var interval = income.Interval.ToTimeSpan(income.IntervalUnits);

                var totalDays = (decimal)interval.TotalDays;
                
                aggregate.Incomes.Add(new NormalizedIncome
                {
                    IncomeId = income.Id,
                    Amount = income.ExactMoney / totalDays
                });
            }
        }
    }
}