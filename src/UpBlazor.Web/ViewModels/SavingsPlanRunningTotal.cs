﻿using Microsoft.AspNetCore.Mvc.Routing;
using UpBlazor.Core.Models;

namespace UpBlazor.Web.ViewModels
{
    public class SavingsPlanRunningTotal : SavingsPlan
    {
        public SavingsPlanRunningTotal(Expense expense, decimal runningTotal)
        {
            Id = expense.Id;
            Amount = expense.Money;
            Name = expense.Name;

            if (expense.FromIncomeId.HasValue)
            {
                IncomeId = expense.FromIncomeId.Value;
            }

            if (expense.FromSaverId is not null)
            {
                SaverId = expense.FromSaverId;
            }

            RunningTotal = runningTotal;
        }
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