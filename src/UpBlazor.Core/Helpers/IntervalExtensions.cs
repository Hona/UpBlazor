using System;
using System.Collections.Generic;
using System.Linq;
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
                Interval.Monthly => TimeSpan.FromDays(units * 30.437), // https://www.britannica.com/science/time/Lengths-of-years-and-months),
                _ => throw new ArgumentOutOfRangeException(nameof(interval), interval, null)
            };

        public static DateTime FindFirstCycle(this DateTime start, Interval interval, int units, DateTime? searchStart = null)
        {
            searchStart ??= DateTime.Now.Date;

            var nowSubtractStart = searchStart - start;
            if (nowSubtractStart.Value.TotalMilliseconds < 0)
            {
                return start;
            }

            var cycleStep = interval.ToTimeSpan(units);
            var currentCycle = start;
            do
            {
                currentCycle = currentCycle.Add(cycleStep);
            } while (currentCycle < searchStart || (searchStart - currentCycle).Value.TotalMilliseconds >= cycleStep.TotalMilliseconds);

            return currentCycle;
        }

        private static List<DateOnly> GetAllCyclesInMonthlyRange(this DateTime start, DateTime rangeStart,
            DateTime rangeEnd, int units)
        {
            var dayOfMonth = start.Day;
    
            var output = new List<DateOnly>();

            var potentialHit = new DateTime(start.Year, start.Month, dayOfMonth);

            while (potentialHit <= rangeEnd)
            {
                if (potentialHit >= rangeStart)
                {
                    output.Add(DateOnly.FromDateTime(potentialHit));
                }
        
                // Increment by the number of months specified in units
                potentialHit = potentialHit.AddMonths(units);
            }

            return output;
        }

        public static List<DateOnly> GetAllCyclesInRange(this DateTime start, DateTime rangeStart, DateTime rangeEnd, Interval interval, int units)
        {
            var startOfRange = start.FindFirstCycle(interval, units, rangeStart);

            // Cycle hasn't started yet
            if (startOfRange > rangeEnd)
            {
                return Enumerable.Empty<DateOnly>().ToList();
            }
            
            if (interval is Interval.Monthly)
            {
                return GetAllCyclesInMonthlyRange(start, rangeStart, rangeEnd, units);
            }

            var output = new List<DateOnly>
            {
                DateOnly.FromDateTime(startOfRange)
            };
            
            // Get values until rangeEnd

            var i = 0;
            while (true)
            {
                i += units;

                var timeSpan = interval.ToTimeSpan(i);

                var potentialHit = startOfRange.Add(timeSpan);

                if (potentialHit <= rangeEnd)
                {
                    output.Add(DateOnly.FromDateTime(potentialHit));
                    continue;
                }
                
                break;
            }

            return output;
        }
    }
}