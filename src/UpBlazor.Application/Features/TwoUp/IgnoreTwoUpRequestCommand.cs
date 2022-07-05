using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UpBlazor.Application.Services;
using UpBlazor.Core.Exceptions;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Application.Features.TwoUp;

public record IgnoreTwoUpRequestCommand(string RequesterId) : IRequest;

public class IgnoreTwoUpRequestCommandHandler : IRequestHandler<IgnoreTwoUpRequestCommand>
{
    private readonly ITwoUpRequestRepository _twoUpRequestRepository;
    private readonly ICurrentUserService _currentUserService;

    public IgnoreTwoUpRequestCommandHandler(ITwoUpRequestRepository twoUpRequestRepository, ICurrentUserService currentUserService)
    {
        _twoUpRequestRepository = twoUpRequestRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(IgnoreTwoUpRequestCommand request, CancellationToken cancellationToken)
    {
        var userId = await _currentUserService.GetUserIdAsync(cancellationToken);

        var requests = await _twoUpRequestRepository.GetAllByRequesteeAsync(userId, cancellationToken);
        
        var toDelete = requests.SingleOrDefault(x => x.RequesterId == request.RequesterId);
        
        if (toDelete is null)
        {
            throw new BadRequestException("No request found");
        }
        
        await _twoUpRequestRepository.DeleteAsync(toDelete, cancellationToken);
        
        return Unit.Value;
    }
}