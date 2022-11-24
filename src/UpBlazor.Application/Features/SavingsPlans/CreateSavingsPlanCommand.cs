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
    private readonly ISavingsPlanRepository _savingsPlanRepository;

    public CreateSavingsPlanCommandHandler(ISavingsPlanRepository savingsPlanRepository)
    {
        _savingsPlanRepository = savingsPlanRepository;
    }

    public async Task<Guid> Handle(CreateSavingsPlanCommand request, CancellationToken cancellationToken)
    {
        var output = new SavingsPlan
        {
            Name = request.Name,
            IncomeId = request.IncomeId,
            SaverId = request.SaverId,
            Amount = request.Amount
        };

        await _savingsPlanRepository.AddAsync(output, cancellationToken);

        return output.Id;
    }
}