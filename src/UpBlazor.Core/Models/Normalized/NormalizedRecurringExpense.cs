using System;

namespace UpBlazor.Core.Models.Normalized
{
    public class NormalizedRecurringExpense
    {
        public Guid RecurringExpenseId { get; set; }
        public decimal Amount { get; set; }
    }
}