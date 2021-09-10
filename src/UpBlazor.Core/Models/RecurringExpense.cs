using System;
using UpBlazor.Core.Interfaces;
using UpBlazor.Core.Models.Enums;

namespace UpBlazor.Core.Models
{
    public class RecurringExpense : ISaverId, IIncomeId
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
        
        /// <summary>
        /// Wraps property FromSaverId
        /// </summary>
        public string SaverId 
        { 
            get => FromSaverId;
            set => FromSaverId = value;
        }

        /// <summary>
        /// Wraps property FromIncomeId
        /// </summary>
        public Guid? InterfaceIncomeId
        {
            get => FromIncomeId;
            set => FromIncomeId = value;
        }
    }
}