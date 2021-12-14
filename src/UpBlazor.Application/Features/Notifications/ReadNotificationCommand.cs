using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UpBlazor.Application.Services;
using UpBlazor.Core.Models;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Application.Features.Notifications;

public record ReadNotificationCommand(Notification Notification) : IRequest<string>;

public class ReadNotificationCommandHandler : IRequestHandler<ReadNotificationCommand, string>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly INotificationReadRepository _notificationReadRepository;

    public ReadNotificationCommandHandler(ICurrentUserService currentUserService, INotificationReadRepository notificationReadRepository)
    {
        _currentUserService = currentUserService;
        _notificationReadRepository = notificationReadRepository;
    }

    public async Task<string> Handle(ReadNotificationCommand request, CancellationToken cancellationToken)
    {
        var userId = await _currentUserService.GetUserIdAsync(cancellationToken);

        var output = new NotificationRead
        {
            NotificationId = request.Notification.Id,
            UserId = userId
        };
        
        await _notificationReadRepository.AddOrUpdateAsync(output, cancellationToken);

        return output.Id;
    }
}