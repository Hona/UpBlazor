using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UpBlazor.Core.Models;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Application.Features.TwoUp;

public record AcceptTwoUpRequestCommand(TwoUpRequest TwoUpRequest) : IRequest;

public class AcceptTwoUpRequestCommandHandler : IRequestHandler<AcceptTwoUpRequestCommand>
{
    private readonly ITwoUpRequestRepository _twoUpRequestRepository;
    private readonly ITwoUpRepository _twoUpRepository;

    public AcceptTwoUpRequestCommandHandler(ITwoUpRequestRepository twoUpRequestRepository, ITwoUpRepository twoUpRepository)
    {
        _twoUpRequestRepository = twoUpRequestRepository;
        _twoUpRepository = twoUpRepository;
    }

    public async Task<Unit> Handle(AcceptTwoUpRequestCommand request, CancellationToken cancellationToken)
    {
        await _twoUpRequestRepository.DeleteAsync(request.TwoUpRequest);

        var twoUp = new Core.Models.TwoUp(request.TwoUpRequest.RequesteeId, request.TwoUpRequest.RequesterId);
        await _twoUpRepository.AddOrUpdateAsync(twoUp);
        
        return Unit.Value;
    }
}