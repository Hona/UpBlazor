using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UpBlazor.Application.Services;

namespace UpBlazor.Application.Features.Normalized;

public record UpdateNormalizedAggregateCommand : IRequest;

public class UpdateNormalizedAggregateCommandHandler : IRequestHandler<UpdateNormalizedAggregateCommand>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly INormalizerService _normalizerService;

    public UpdateNormalizedAggregateCommandHandler(ICurrentUserService currentUserService, INormalizerService normalizerService)
    {
        _currentUserService = currentUserService;
        _normalizerService = normalizerService;
    }

    public async Task<Unit> Handle(UpdateNormalizedAggregateCommand request, CancellationToken cancellationToken)
    {
        var userId = await _currentUserService.GetUserIdAsync();
        await _normalizerService.UpdateUserAsync(userId);
        
        return Unit.Value;
    }
}