using UpBlazor.Core.Models;

namespace UpBlazor.Web.ViewModels
{
    public class IncomeGoalRunningTotal : IncomeGoal
    {
        public IncomeGoalRunningTotal(IncomeGoal incomeGoal, decimal runningTotal)
        {
            Id = incomeGoal.Id;
            Amount = incomeGoal.Amount;
            Name = incomeGoal.Name;
            IncomeId = incomeGoal.IncomeId;
            SaverId = incomeGoal.SaverId;
            
            RunningTotal = runningTotal;
        }
        public decimal RunningTotal { get; set; }
    }
}