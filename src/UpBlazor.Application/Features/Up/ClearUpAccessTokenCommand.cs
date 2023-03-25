using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UpBlazor.Application.Services;
using UpBlazor.Domain.Repositories;

namespace UpBlazor.Application.Features.Up;

public record ClearUpAccessTokenCommand : IRequest;

public class ClearUpAccessTokenCommandHandler : IRequestHandler<ClearUpAccessTokenCommand>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IUpUserTokenRepository _upUserTokenRepository;
    
    public ClearUpAccessTokenCommandHandler(ICurrentUserService currentUserService, IUpUserTokenRepository upUserTokenRepository)
    {
        _currentUserService = currentUserService;
        _upUserTokenRepository = upUserTokenRepository;
    }

    public async Task<Unit> Handle(ClearUpAccessTokenCommand request, CancellationToken cancellationToken)
    {
        var userId = await _currentUserService.GetUserIdAsync(cancellationToken);
        var existingToken = await _upUserTokenRepository.GetByUserIdAsync(userId, cancellationToken);

        if (existingToken != null)
        {
            await _upUserTokenRepository.DeleteAsync(existingToken, cancellationToken);
        }
        
        return Unit.Value;
    }
}