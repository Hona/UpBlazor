using System;

namespace UpBlazor.Core.Models
{
    public class Goal
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string SaverId { get; set; }
    }
}