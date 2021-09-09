using System;
using UpBlazor.Core.Models.Enums;

namespace UpBlazor.Core.Models
{
    public class RecurringExpense
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public Interval Interval { get; set; }
        public int IntervalUnits { get; set; }
        public Money Money { get; set; }
        public string FromSaverId { get; set; }
        public Guid? FromIncomeId { get; set; }
    }
}