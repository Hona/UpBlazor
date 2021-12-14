using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UpBlazor.Core.Models;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Application.Features.TwoUp;

public record IgnoreTwoUpRequestCommand(TwoUpRequest TwoUpRequest) : IRequest;

public class IgnoreTwoUpRequestCommandHandler : IRequestHandler<IgnoreTwoUpRequestCommand>
{
    private readonly ITwoUpRequestRepository _twoUpRequestRepository;

    public IgnoreTwoUpRequestCommandHandler(ITwoUpRequestRepository twoUpRequestRepository)
    {
        _twoUpRequestRepository = twoUpRequestRepository;
    }

    public async Task<Unit> Handle(IgnoreTwoUpRequestCommand request, CancellationToken cancellationToken)
    {
        await _twoUpRequestRepository.DeleteAsync(request.TwoUpRequest);
        
        return Unit.Value;
    }
}