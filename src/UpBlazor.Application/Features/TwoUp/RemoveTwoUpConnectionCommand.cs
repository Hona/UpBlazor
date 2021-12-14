using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UpBlazor.Application.Services;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Application.Features.TwoUp;

public record RemoveTwoUpConnectionCommand(string UserId) : IRequest;

public class RemoveTwoUpConnectionCommandHandler : IRequestHandler<RemoveTwoUpConnectionCommand>
{
    private ITwoUpRepository _twoUpRepository;
    private ICurrentUserService _currentUserService;

    public RemoveTwoUpConnectionCommandHandler(ITwoUpRepository twoUpRepository, ICurrentUserService currentUserService)
    {
        _twoUpRepository = twoUpRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(RemoveTwoUpConnectionCommand request, CancellationToken cancellationToken)
    {
        var userId = await _currentUserService.GetUserIdAsync();
        
        var toRemove = new Core.Models.TwoUp(userId, request.UserId);

        await _twoUpRepository.DeleteAsync(toRemove);
        
        return Unit.Value;
    }
}