using System.Collections.Generic;
using UpBlazor.Core.Models.Normalized;

namespace UpBlazor.Core.Models
{
    public class NormalizedAggregate
    {
        public NormalizedAggregate(string userId)
        {
            Incomes = new();
            RecurringExpenses = new();
            IncomeGoals = new();
            UserId = userId;
        }
        
        public string UserId { get; set; }
        public List<NormalizedIncome> Incomes { get; set; }
        public List<NormalizedRecurringExpense> RecurringExpenses { get; set; }
        public List<NormalizedIncomeGoal> IncomeGoals { get; set; }
    }
}