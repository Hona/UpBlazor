using UpBlazor.Core.Models;

namespace UpBlazor.Web.ViewModels
{
    public class SavingsPlanRunningTotal : SavingsPlan
    {
        public SavingsPlanRunningTotal(SavingsPlan savingsPlan, decimal runningTotal)
        {
            Id = savingsPlan.Id;
            Amount = savingsPlan.Amount;
            Name = savingsPlan.Name;
            IncomeId = savingsPlan.IncomeId;
            SaverId = savingsPlan.SaverId;
            
            RunningTotal = runningTotal;
        }
        public decimal RunningTotal { get; set; }
    }
}