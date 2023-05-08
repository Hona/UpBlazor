using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UpBlazor.Application.Repositories;
using UpBlazor.Domain.Entities;

namespace UpBlazor.Application.Features.Notifications;

public record GetAllNotificationsQuery : IRequest<IReadOnlyList<Notification>>;

public class GetAllNotificationsQueryHandler : IRequestHandler<GetAllNotificationsQuery, IReadOnlyList<Notification>>
{
    private readonly INotificationRepository _notificationRepository;

    public GetAllNotificationsQueryHandler(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task<IReadOnlyList<Notification>> Handle(GetAllNotificationsQuery request, CancellationToken cancellationToken)
    {
        var output = await _notificationRepository.GetAllAsync(cancellationToken);

        return output;
    }
}

