using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UpBlazor.Core.Models;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Application.Features.Goals;

public record DeleteGoalCommand(Goal Goal) : IRequest;

public class DeleteGoalCommandHandler : IRequestHandler<DeleteGoalCommand>
{
    private readonly IGoalRepository _goalRepository;

    public DeleteGoalCommandHandler(IGoalRepository goalRepository)
    {
        _goalRepository = goalRepository;
    }

    public async Task<Unit> Handle(DeleteGoalCommand request, CancellationToken cancellationToken)
    {
        await _goalRepository.DeleteAsync(request.Goal, cancellationToken);
        
        return Unit.Value;
    }
}
