using System;
using UpBlazor.Domain.Common.Interfaces;
using UpBlazor.Domain.Common.Models;

namespace UpBlazor.Domain.Entities
{
    public class Expense : ISaverId, IIncomeId
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public Money Money { get; set; }
        public DateTime PaidByDate { get; set; }

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