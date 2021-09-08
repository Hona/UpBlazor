using System;

namespace UpBlazor.Core.Models
{
    public class IncomeGoal
    {
        public Guid Id { get; set; }
        public Guid IncomeId { get; set; }

        public string Name { get; set; }
        public Money Amount { get; set; }
    }
}