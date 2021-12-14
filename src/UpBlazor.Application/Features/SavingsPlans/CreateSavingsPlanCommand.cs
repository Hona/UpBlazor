using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UpBlazor.Application.Services;
using UpBlazor.Core.Models;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Application.Features.SavingsPlans;

public record CreateSavingsPlanCommand(Guid IncomeId, string Name, string SaverId, Money Amount) : IRequest<Guid>;

public class CreateSavingsPlanCommandHandler : IRequestHandler<CreateSavingsPlanCommand, Guid>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly ISavingsPlanRepository _savingsPlanRepository;

    public CreateSavingsPlanCommandHandler(ICurrentUserService currentUserService, ISavingsPlanRepository savingsPlanRepository)
    {
        _currentUserService = currentUserService;
        _savingsPlanRepository = savingsPlanRepository;
    }

    public async Task<Guid> Handle(CreateSavingsPlanCommand request, CancellationToken cancellationToken)
    {
        if (request.Amount.Percent.HasValue)
        {
            request.Amount.Percent /= 100;
        }

        var output = new Core.Models.SavingsPlan
        {
            Name = request.Name,
            IncomeId = request.IncomeId,
            SaverId = request.SaverId,
            Amount = request.Amount
        };

        await _savingsPlanRepository.AddAsync(output);

        return output.Id;
    }
}