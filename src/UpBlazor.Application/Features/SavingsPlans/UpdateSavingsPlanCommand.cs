using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UpBlazor.Application.Features.Expenses;
using UpBlazor.Core.Models;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Application.Features.SavingsPlans;

public record UpdateSavingsPlanCommand(SavingsPlan SavingsPlan) : IRequest;

public class UpdateSavingsPlanCommandHandler : IRequestHandler<UpdateSavingsPlanCommand>
{
    private readonly ISavingsPlanRepository _savingsPlanRepository;

    public UpdateSavingsPlanCommandHandler(ISavingsPlanRepository savingsPlanRepository)
    {
        _savingsPlanRepository = savingsPlanRepository;
    }

    public async Task<Unit> Handle(UpdateSavingsPlanCommand request, CancellationToken cancellationToken)
    {
        await _savingsPlanRepository.UpdateAsync(request.SavingsPlan, cancellationToken);

        return Unit.Value;
    }
}