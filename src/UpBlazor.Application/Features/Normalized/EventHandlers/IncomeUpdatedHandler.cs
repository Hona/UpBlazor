using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UpBlazor.Application.Common.Services;
using UpBlazor.Domain.Events;

namespace UpBlazor.Application.Features.Normalized.EventHandlers;

public class IncomeUpdatedHandler : INotificationHandler<IncomeUpdatedEvent>
{
    private readonly INormalizerService _normalizerService;

    public IncomeUpdatedHandler(INormalizerService normalizerService) => _normalizerService = normalizerService;

    public async Task Handle(IncomeUpdatedEvent notification, CancellationToken cancellationToken)
    {
        await _normalizerService.UpdateUserAsync(notification.Income.UserId, cancellationToken);
    }
}