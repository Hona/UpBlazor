using System.Collections.Generic;

namespace UpBlazor.Domain.Entities.Normalized
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