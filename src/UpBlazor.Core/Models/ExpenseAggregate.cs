using System.Collections.Generic;

namespace UpBlazor.Core.Models
{
    public class ExpenseAggregate
    {
        public IReadOnlyList<Expense> Expenses { get; set; }
        public IReadOnlyList<RecurringExpense> RecurringExpenses { get; set; }
    }
}