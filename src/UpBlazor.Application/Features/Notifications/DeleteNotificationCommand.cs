using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UpBlazor.Core.Models;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Application.Features.Notifications;

public record DeleteNotificationCommand(Notification Notification) : IRequest;

public class DeleteNotificationCommandHandler : IRequestHandler<DeleteNotificationCommand>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly INotificationReadRepository _notificationReadRepository;

    public DeleteNotificationCommandHandler(INotificationRepository notificationRepository, INotificationReadRepository notificationReadRepository)
    {
        _notificationRepository = notificationRepository;
        _notificationReadRepository = notificationReadRepository;
    }

    public async Task<Unit> Handle(DeleteNotificationCommand request, CancellationToken cancellationToken)
    {
        await _notificationRepository.DeleteAsync(request.Notification);

        var notificationId = request.Notification.Id;
        var notificationReads = await _notificationReadRepository.GetByNotificationIdAsync(notificationId);

        foreach (var notificationRead in notificationReads)
        {
            await _notificationReadRepository.DeleteAsync(notificationRead);
        }
        
        return Unit.Value;
    }
}