using System;
using UpBlazor.Core.Models.Enums;

namespace UpBlazor.Core.Helpers
{
    public static class IntervalExtensions
    {
        public static TimeSpan ToTimeSpan(this Interval interval, int units) =>
            interval switch
            {
                Interval.Days => TimeSpan.FromDays(units),
                Interval.Weeks => TimeSpan.FromDays(units * 7),
                Interval.Fortnights => TimeSpan.FromDays(units * 14),
                _ => throw new ArgumentOutOfRangeException(nameof(interval), interval, null)
            };

        public static DateTime FindLastCycleStart(this DateTime start, Interval interval, int units)
        {
            var now = DateTime.Now;

            var nowSubtractStart = now - start;
            if (nowSubtractStart.TotalMilliseconds < 0)
            {
                return start;
            }

            var cycleStep = interval.ToTimeSpan(units);
            var currentCycle = start;
            do
            {
                currentCycle = currentCycle.Add(cycleStep);
            } while ((now - currentCycle).TotalMilliseconds >= cycleStep.TotalMilliseconds);

            return currentCycle;
        }
    }
}