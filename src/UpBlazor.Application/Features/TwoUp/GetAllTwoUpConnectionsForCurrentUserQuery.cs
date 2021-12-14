using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UpBlazor.Application.Services;
using UpBlazor.Core.Models;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Application.Features.TwoUp;

public record GetAllTwoUpConnectionsForCurrentUserQuery : IRequest<IReadOnlyList<RegisteredUser>>;

public class GetAllTwoUpConnectionsForCurrentUserQueryHandler : IRequestHandler<GetAllTwoUpConnectionsForCurrentUserQuery, IReadOnlyList<RegisteredUser>>
{
    private readonly ITwoUpRepository _twoUpRepository;
    private readonly IRegisteredUserRepository _registeredUserRepository;
    private readonly ICurrentUserService _currentUserService;
    
    public GetAllTwoUpConnectionsForCurrentUserQueryHandler(ITwoUpRepository twoUpRepository, ICurrentUserService currentUserService, IRegisteredUserRepository registeredUserRepository)
    {
        _twoUpRepository = twoUpRepository;
        _currentUserService = currentUserService;
        _registeredUserRepository = registeredUserRepository;
    }

    public async Task<IReadOnlyList<RegisteredUser>> Handle(GetAllTwoUpConnectionsForCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var userId = await _currentUserService.GetUserIdAsync();
        var twoUpConnections = await _twoUpRepository.GetAllByUserIdAsync(userId);

        var twoUpConnectionUserIds = twoUpConnections.SelectMany(x => new[]
            {
                x.UserId1,
                x.UserId2
            }).Distinct()
            .Where(x => x != userId)
            .ToArray();

        var twoUpConnectionUsers = await _registeredUserRepository.GetAllByIdsAsync(twoUpConnectionUserIds);
        return twoUpConnectionUsers;
    }
}