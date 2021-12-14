using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UpBlazor.Application.Services;
using UpBlazor.Core.Models;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Application.Features.Goals;

public record CreateGoalCommand(string Name, DateTime Date, decimal Amount, string SaverId) : IRequest<Guid>;

public class CreateGoalCommandHandler : IRequestHandler<CreateGoalCommand, Guid>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IGoalRepository _goalRepository;

    public CreateGoalCommandHandler(ICurrentUserService currentUserService, IGoalRepository goalRepository)
    {
        _currentUserService = currentUserService;
        _goalRepository = goalRepository;
    }

    public async Task<Guid> Handle(CreateGoalCommand request, CancellationToken cancellationToken)
    {
        var userId = await _currentUserService.GetUserIdAsync(cancellationToken);

        var output = new Goal
        {
            Name = request.Name,
            Amount = request.Amount,
            Date = request.Date,
            SaverId = request.SaverId,
            UserId = userId
        };

        await _goalRepository.AddAsync(output, cancellationToken);

        return output.Id;
    }
}
