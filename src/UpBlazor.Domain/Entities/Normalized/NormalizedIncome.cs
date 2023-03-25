using System;

namespace UpBlazor.Domain.Entities.Normalized
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