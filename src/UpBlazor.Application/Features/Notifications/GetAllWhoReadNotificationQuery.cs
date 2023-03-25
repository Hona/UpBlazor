using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UpBlazor.Domain.Models;
using UpBlazor.Domain.Repositories;

namespace UpBlazor.Application.Features.Notifications;

public record GetAllWhoReadNotificationQuery(Notification Notification) : IRequest<IReadOnlyList<RegisteredUser>>;

public class GetAllWhoReadNotificationQueryHandler : IRequestHandler<GetAllWhoReadNotificationQuery, IReadOnlyList<RegisteredUser>>
{
    private readonly IRegisteredUserRepository _registeredUserRepository;
    private readonly INotificationReadRepository _notificationReadRepository;

    public GetAllWhoReadNotificationQueryHandler(IRegisteredUserRepository registeredUserRepository, INotificationReadRepository notificationReadRepository)
    {
        _registeredUserRepository = registeredUserRepository;
        _notificationReadRepository = notificationReadRepository;
    }

    public async Task<IReadOnlyList<RegisteredUser>> Handle(GetAllWhoReadNotificationQuery request, CancellationToken cancellationToken)
    {
        var notificationId = request.Notification.Id;
        var readReceipts = await _notificationReadRepository.GetByNotificationIdAsync(notificationId, cancellationToken);

        var userIds = readReceipts
            .Select(x => x.UserId)
            .ToArray();

        var output = await _registeredUserRepository.GetAllByIdsAsync(cancellationToken, userIds);
        return output;
    }
}