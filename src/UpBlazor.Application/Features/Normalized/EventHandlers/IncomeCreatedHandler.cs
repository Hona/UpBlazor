using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UpBlazor.Application.Common.Services;
using UpBlazor.Domain.Events;

namespace UpBlazor.Application.Features.Normalized.EventHandlers;

public class IncomeCreatedHandler : INotificationHandler<IncomeCreatedEvent>
{
    private readonly INormalizerService _normalizerService;

    public IncomeCreatedHandler(INormalizerService normalizerService) => _normalizerService = normalizerService;

    public async Task Handle(IncomeCreatedEvent notification, CancellationToken cancellationToken)
    {
        await _normalizerService.UpdateUserAsync(notification.Income.UserId, cancellationToken);
    }
}