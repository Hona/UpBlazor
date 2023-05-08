using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UpBlazor.Application.Common.Services;
using UpBlazor.Domain.Events;

namespace UpBlazor.Application.Features.Normalized.EventHandlers;

public class RecurringExpenseDeletedHandler : INotificationHandler<RecurringExpenseDeletedEvent>
{
    private readonly INormalizerService _normalizerService;

    public RecurringExpenseDeletedHandler(INormalizerService normalizerService) => _normalizerService = normalizerService;

    public async Task Handle(RecurringExpenseDeletedEvent notification, CancellationToken cancellationToken)
    {
        await _normalizerService.UpdateUserAsync(notification.RecurringExpense.UserId, cancellationToken);
    }
}