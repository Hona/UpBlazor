using System;
using UpBlazor.Domain.Common.Interfaces;
using UpBlazor.Domain.Common.Models;
using UpBlazor.Domain.Entities.Enums;

namespace UpBlazor.Domain.Entities
{
    public class RecurringExpense : ISaverId
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public Interval Interval { get; set; }
        public int IntervalUnits { get; set; }
        public Money Money { get; set; }
        public string FromSaverId { get; set; }

        /// <summary>
        /// Wraps property FromSaverId
        /// </summary>
        public string SaverId 
        { 
            get => FromSaverId;
            set => FromSaverId = value;
        }
        
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