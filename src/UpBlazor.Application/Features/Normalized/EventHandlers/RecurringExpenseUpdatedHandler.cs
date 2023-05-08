using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UpBlazor.Application.Common.Services;
using UpBlazor.Domain.Events;

namespace UpBlazor.Application.Features.Normalized.EventHandlers;

public class RecurringExpenseUpdatedHandler : INotificationHandler<RecurringExpenseUpdatedEvent>
{
    private readonly INormalizerService _normalizerService;

    public RecurringExpenseUpdatedHandler(INormalizerService normalizerService) => _normalizerService = normalizerService;

    public async Task Handle(RecurringExpenseUpdatedEvent notification, CancellationToken cancellationToken)
    {
        await _normalizerService.UpdateUserAsync(notification.RecurringExpense.UserId, cancellationToken);
    }
}