using System;
using UpBlazor.Domain.Entities.Enums;

namespace UpBlazor.Domain.Entities
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

        public bool FallsOn(DateTime dateTime, DateTime startDate, out int totalCyclesSinceStart)
        {
            var date = dateTime.Date;
            
            if (date < startDate)
            {
                totalCyclesSinceStart = default;
                return false;
            }

            if (date == startDate)
            {
                totalCyclesSinceStart = 0;
                return true;
            }

            var loopDate = startDate.Date;

            totalCyclesSinceStart = 0;
            
            do
            {
                totalCyclesSinceStart++;
                var toAdd = Interval switch
                {
                    Interval.Days => TimeSpan.FromDays(IntervalUnits),
                    Interval.Fortnights => TimeSpan.FromDays(IntervalUnits * 14),
                    Interval.Weeks => TimeSpan.FromDays(7),
                    _ => throw new ArgumentOutOfRangeException()
                };

                loopDate = loopDate.Add(toAdd);

                if (loopDate.Date == date.Date)
                {
                    return true;
                }
            } while (loopDate < date);

            return false;
        }
    }
}