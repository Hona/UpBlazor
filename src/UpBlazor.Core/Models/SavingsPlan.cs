using System;
using UpBlazor.Core.Interfaces;

namespace UpBlazor.Core.Models
{
    public class SavingsPlan : ISaverId, IIncomeId
    {
        public Guid Id { get; set; }
        public Guid IncomeId { get; set; }

        public string Name { get; set; }
        public Money Amount { get; set; }
        public string SaverId { get; set; }
        
        /// <summary>
        /// Wraps IncomeId
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Null is not supported</exception>
        public Guid? InterfaceIncomeId
        {
            get => IncomeId;
            set => IncomeId = value ?? throw new ArgumentOutOfRangeException(nameof(value), "Cannot bind nullable value to this property");
        }
    }
}