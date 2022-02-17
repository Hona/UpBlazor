using System;

namespace UpBlazor.Application.Features.Forecast;

public class ForecastDto
{
    public string cycle { get; set; }
    public decimal balance { get; set; }
    public string accountName { get; set; }
    public int Index { get; set; }
    public Guid? ExpenseId { get; set; }
    public Guid? RecurringExpenseId { get; set; }
    public string UpAccountId { get; set; }
    public int SortPriority { get; set; }
    public ForecastDto Clone() => (ForecastDto)MemberwiseClone();
}