using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UpBlazor.Application.Services;
using UpBlazor.Core.Models;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Application.Features.Normalized;

public record GetNormalizedAggregateQuery : IRequest<NormalizedAggregate>;

public class GetNormalizedAggregateQueryHandler : IRequestHandler<GetNormalizedAggregateQuery, NormalizedAggregate>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly INormalizedAggregateRepository _normalizedAggregateRepository;
    private readonly IMediator _mediator;
    
    public GetNormalizedAggregateQueryHandler(INormalizedAggregateRepository normalizedAggregateRepository, IMediator mediator, ICurrentUserService currentUserService)
    {
        _normalizedAggregateRepository = normalizedAggregateRepository;
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    public async Task<NormalizedAggregate> Handle(GetNormalizedAggregateQuery request, CancellationToken cancellationToken)
    {
        await _mediator.Send(new UpdateNormalizedAggregateCommand(), cancellationToken);

        var userId = await _currentUserService.GetUserIdAsync();
        
        var output = await _normalizedAggregateRepository.GetByUserIdAsync(userId);
        return output;
    }
}