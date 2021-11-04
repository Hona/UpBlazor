using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UpBlazor.Core.Helpers;
using UpBlazor.Core.Models;
using UpBlazor.Core.Models.Normalized;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Application.Services
{
    public class NormalizerService : INormalizerService
    {
        private readonly IIncomeRepository _incomeRepository;
        private readonly ISavingsPlanRepository _savingsPlanRepository;
        private readonly IRecurringExpenseRepository _recurringExpenseRepository;

        private readonly INormalizedAggregateRepository _normalizedAggregateRepository;

        public NormalizerService(IIncomeRepository incomeRepository, ISavingsPlanRepository savingsPlanRepository, IRecurringExpenseRepository recurringExpenseRepository, INormalizedAggregateRepository normalizedAggregateRepository)
        {
            _incomeRepository = incomeRepository;
            _savingsPlanRepository = savingsPlanRepository;
            _recurringExpenseRepository = recurringExpenseRepository;
            _normalizedAggregateRepository = normalizedAggregateRepository;
        }

        public async Task UpdateUserAsync(string userId)
        {
            var incomesTask = _incomeRepository.GetAllByUserIdAsync(userId);
            var savingsPlansTask = _savingsPlanRepository.GetAllByUserIdAsync(userId);
            var recurringExpensesTask = _recurringExpenseRepository.GetAllByUserIdAsync(userId);

            var incomes = await incomesTask;
            var savingsPlans = await savingsPlansTask;
            var recurringExpenses = await recurringExpensesTask;

            var aggregate = new NormalizedAggregate(userId);

            NormalizeIncomes(aggregate, incomes);
            NormalizeSavingsPlans(aggregate, savingsPlans, incomes);
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
                    amount += recurringExpense.Money.Exact.Value / totalDays;
                }

                if (recurringExpense.Money.Percent.HasValue)
                {
                    throw new NotImplementedException(
                        "Currently cannot normalize percent based saver recurring expenses");
                }

                output.RecurringExpenses.Add(new NormalizedRecurringExpense
                {
                    RecurringExpenseId = recurringExpense.Id,
                    Amount = amount
                });
            }
        }

        private static void NormalizeSavingsPlans(NormalizedAggregate output, IEnumerable<SavingsPlan> savingsPlans, IReadOnlyList<Income> incomes)
        {
            foreach (var savingsPlan in savingsPlans)
            {
                var income = incomes.First(x => x.Id == savingsPlan.IncomeId);

                var interval = income.Interval.ToTimeSpan(income.IntervalUnits);

                var totalDays = (decimal)interval.TotalDays;

                var unnormalizedAmount = 0M;

                if (savingsPlan.Amount.Exact.HasValue)
                {
                    unnormalizedAmount += savingsPlan.Amount.Exact.Value;
                }

                if (savingsPlan.Amount.Percent.HasValue)
                {
                    unnormalizedAmount += savingsPlan.Amount.Percent.Value * income.ExactMoney;
                }

                output.SavingsPlans.Add(new NormalizedSavingsPlan
                {
                    SavingsPlanId = savingsPlan.Id,
                    Amount = unnormalizedAmount / totalDays
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