using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UpBlazor.Application.Services;
using UpBlazor.Core.Models;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Application.Features.Notifications;

public record GetAllNotificationsForCurrentUserQuery(bool IncludeRead = false) : IRequest<IReadOnlyList<Notification>>;

public class GetAllNotificationsForCurrentUserQueryHandler : IRequestHandler<GetAllNotificationsForCurrentUserQuery, IReadOnlyList<Notification>>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly INotificationReadRepository _notificationReadRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetAllNotificationsForCurrentUserQueryHandler(INotificationRepository notificationRepository, INotificationReadRepository notificationReadRepository, ICurrentUserService currentUserService)
    {
        _notificationRepository = notificationRepository;
        _notificationReadRepository = notificationReadRepository;
        _currentUserService = currentUserService;
    }

    public async Task<IReadOnlyList<Notification>> Handle(GetAllNotificationsForCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var userId = await _currentUserService.GetUserIdAsync();
        
        var readNotifications = await _notificationReadRepository.GetByUserIdAsync(userId);
        var allNotifications = await _notificationRepository.GetAllAsync();

        if (request.IncludeRead)
        {
            return allNotifications;
        }
        
        var output = allNotifications
            .Where(x => readNotifications.All(read => read.NotificationId != x.Id))
            .ToList()
            .AsReadOnly();

        return output;
    }
}
