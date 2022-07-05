using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UpBlazor.Application.Services;
using UpBlazor.Core.Exceptions;
using UpBlazor.Core.Models;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Application.Features.TwoUp;

public record AcceptTwoUpRequestCommand(TwoUpRequest TwoUpRequest) : IRequest;

public class AcceptTwoUpRequestCommandHandler : IRequestHandler<AcceptTwoUpRequestCommand>
{
    private readonly ITwoUpRequestRepository _twoUpRequestRepository;
    private readonly ITwoUpRepository _twoUpRepository;
    private readonly ICurrentUserService _currentUserService;

    public AcceptTwoUpRequestCommandHandler(ITwoUpRequestRepository twoUpRequestRepository, ITwoUpRepository twoUpRepository, ICurrentUserService currentUserService)
    {
        _twoUpRequestRepository = twoUpRequestRepository;
        _twoUpRepository = twoUpRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(AcceptTwoUpRequestCommand request, CancellationToken cancellationToken)
    {
        var userId = await _currentUserService.GetUserIdAsync(cancellationToken);

        if (userId != request.TwoUpRequest.RequesteeId)
        {
            throw new BadRequestException("Request not found");
        }
        
        await _twoUpRequestRepository.DeleteAsync(request.TwoUpRequest, cancellationToken);

        var twoUp = new Core.Models.TwoUp(request.TwoUpRequest.RequesteeId, request.TwoUpRequest.RequesterId);
        await _twoUpRepository.AddOrUpdateAsync(twoUp, cancellationToken);
        
        return Unit.Value;
    }
}