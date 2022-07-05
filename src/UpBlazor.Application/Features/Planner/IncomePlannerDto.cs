using System;
using System.Collections.Generic;
using Up.NET.Api.Accounts;

namespace UpBlazor.Application.Features.Planner;

public class IncomePlannerDto
{
    public List<SavingsPlanRunningTotal> IncomeExpenseSubTotals { get; set; }
    public List<SavingsPlanRunningTotal> ProRataExpenseSubTotals { get; set; }
    public List<SavingsPlanRunningTotal> ExactSavingsPlanSubTotals { get; set; }
    public List<SavingsPlanRunningTotal> PercentSavingsPlanSubTotals { get; set; }

    public decimal UnbudgetedMoney { get; set; }

    public Dictionary<string, decimal> FinalBudget { get; set; }
}