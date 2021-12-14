using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UpBlazor.Application.Services;
using UpBlazor.Core.Models;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Application.Features.TwoUp;

public record GetAllTwoUpRequestsForCurrentUserQuery : IRequest<IReadOnlyList<TwoUpRequest>>;

public class GetAllTwoUpRequestsForCurrentUserQueryHandler : IRequestHandler<GetAllTwoUpRequestsForCurrentUserQuery, IReadOnlyList<TwoUpRequest>>
{
    private readonly ITwoUpRequestRepository _twoUpRequestRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetAllTwoUpRequestsForCurrentUserQueryHandler(ITwoUpRequestRepository twoUpRequestRepository, ICurrentUserService currentUserService)
    {
        _twoUpRequestRepository = twoUpRequestRepository;
        _currentUserService = currentUserService;
    }

    public async Task<IReadOnlyList<TwoUpRequest>> Handle(GetAllTwoUpRequestsForCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var userId = await _currentUserService.GetUserIdAsync();

        var output = await _twoUpRequestRepository.GetAllByRequesteeAsync(userId);

        return output;
    }
}