using System;
using UpBlazor.Core.Models.Enums;

namespace UpBlazor.Core.Models
{
    public class Income
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public decimal ExactMoney { get; set; }
        public Interval Interval { get; set; }
        public int IntervalUnits { get; set; }

        public override string ToString() => $"{Name} (${ExactMoney:F2} every {IntervalUnits} {Interval})";
    }
}