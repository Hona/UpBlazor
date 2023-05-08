using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UpBlazor.Application.Common.Services;
using UpBlazor.Domain.Events;

namespace UpBlazor.Application.Features.Normalized.EventHandlers;

public class RecurringExpenseCreatedHandler : INotificationHandler<RecurringExpenseCreatedEvent>
{
    private readonly INormalizerService _normalizerService;

    public RecurringExpenseCreatedHandler(INormalizerService normalizerService) => _normalizerService = normalizerService;

    public async Task Handle(RecurringExpenseCreatedEvent notification, CancellationToken cancellationToken)
    {
        await _normalizerService.UpdateUserAsync(notification.RecurringExpense.UserId, cancellationToken);
    }
}