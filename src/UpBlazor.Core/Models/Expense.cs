using System;

namespace UpBlazor.Core.Models
{
    public class Expense
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Money Money { get; set; }
        public DateTime PaidByDate { get; set; }
    }
}