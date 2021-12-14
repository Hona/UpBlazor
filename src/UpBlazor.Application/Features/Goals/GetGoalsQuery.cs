using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UpBlazor.Application.Services;
using UpBlazor.Core.Models;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Application.Features.Goals;

public record GetGoalsQuery : IRequest<IReadOnlyList<Goal>>;

public class GetGoalsQueryHandler : IRequestHandler<GetGoalsQuery, IReadOnlyList<Goal>>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IGoalRepository _goalRepository;

    public GetGoalsQueryHandler(ICurrentUserService currentUserService, IGoalRepository goalRepository)
    {
        _currentUserService = currentUserService;
        _goalRepository = goalRepository;
    }

    public async Task<IReadOnlyList<Goal>> Handle(GetGoalsQuery request, CancellationToken cancellationToken)
    {
        var userId = await _currentUserService.GetUserIdAsync();

        var output = await _goalRepository.GetAllByUserIdAsync(userId);
        return output;
    }
}
