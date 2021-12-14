using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UpBlazor.Core.Models;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Application.Features.SavingsPlans;

public record GetSavingsPlansQuery(Guid IncomeId) : IRequest<IReadOnlyList<SavingsPlan>>;

public class GetSavingsPlansQueryHandler : IRequestHandler<GetSavingsPlansQuery, IReadOnlyList<SavingsPlan>>
{
    private readonly ISavingsPlanRepository _savingsPlanRepository;

    public GetSavingsPlansQueryHandler(ISavingsPlanRepository savingsPlanRepository)
    {
        _savingsPlanRepository = savingsPlanRepository;
    }

    public async Task<IReadOnlyList<SavingsPlan>> Handle(GetSavingsPlansQuery request, CancellationToken cancellationToken)
    {
        var output = await _savingsPlanRepository.GetAllByIncomeIdAsync(request.IncomeId);
        return output;
    }
}