using System;

namespace UpBlazor.Domain.Models.Normalized
{
    /// <summary>
    /// All values are normalized to **per-day**
    /// </summary>
    public class NormalizedIncome
    {
        public Guid IncomeId { get; set; }
        public decimal Amount { get; set; }
    }
}