using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace UpBlazor.Application.Features.Notifications;

public record GetUnreadNotificationCountQuery : IRequest<int>;

public class GetUnreadNotificationCountQueryHandler : IRequestHandler<GetUnreadNotificationCountQuery, int>
{
    private readonly IMediator _mediator;

    public GetUnreadNotificationCountQueryHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<int> Handle(GetUnreadNotificationCountQuery request, CancellationToken cancellationToken)
    {
        var systemNotifications = await _mediator.Send(new GetAllNotificationsForCurrentUserQuery(), cancellationToken);

        var output = systemNotifications.Count;
        return output;
    }
}