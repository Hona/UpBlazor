using System.Collections.Generic;
using UpBlazor.Domain.Models.Normalized;

namespace UpBlazor.Domain.Models
{
    public class NormalizedAggregate
    {
        public NormalizedAggregate(string userId)
        {
            Incomes = new();
            RecurringExpenses = new();
            SavingsPlans = new();
            UserId = userId;
        }
        
        public string UserId { get; set; }
        public List<NormalizedIncome> Incomes { get; set; }
        public List<NormalizedRecurringExpense> RecurringExpenses { get; set; }
        public List<NormalizedSavingsPlan> SavingsPlans { get; set; }
    }
}