using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace UpBlazor.Application.Common.Services;

public interface IForecastService
{
    Task<Dictionary<Guid, List<DateOnly>>> GetRecurringExpenseCyclesInRangeAsync(DateTime rangeStart, DateTime rangeEnd, CancellationToken cancellationToken = default);

    Task<Dictionary<Guid, List<DateOnly>>> GetIncomeCyclesInRangeAsync(DateTime rangeStart, DateTime rangeEnd, CancellationToken cancellationToken = default);
}