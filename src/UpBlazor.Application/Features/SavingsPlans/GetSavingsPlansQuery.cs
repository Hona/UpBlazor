using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UpBlazor.Application.Services;
using UpBlazor.Domain.Exceptions;
using UpBlazor.Domain.Models;
using UpBlazor.Domain.Repositories;

namespace UpBlazor.Application.Features.SavingsPlans;

public record GetSavingsPlansQuery(Guid IncomeId) : IRequest<IReadOnlyList<SavingsPlan>>;

public class GetSavingsPlansQueryHandler : IRequestHandler<GetSavingsPlansQuery, IReadOnlyList<SavingsPlan>>
{
    private readonly ISavingsPlanRepository _savingsPlanRepository;
    private readonly IIncomeRepository _incomeRepository;
    private readonly ICurrentUserService _currentUserService;
    
    public GetSavingsPlansQueryHandler(ISavingsPlanRepository savingsPlanRepository, IIncomeRepository incomeRepository, ICurrentUserService currentUserService)
    {
        _savingsPlanRepository = savingsPlanRepository;
        _incomeRepository = incomeRepository;
        _currentUserService = currentUserService;
    }

    public async Task<IReadOnlyList<SavingsPlan>> Handle(GetSavingsPlansQuery request, CancellationToken cancellationToken)
    {
        var userId = await _currentUserService.GetUserIdAsync(cancellationToken);

        var userIncomes = await _incomeRepository.GetAllByUserIdAsync(userId, cancellationToken);

        if (userIncomes.All(x => x.Id != request.IncomeId))
        {
            throw new BadRequestException("Income not found");
        }
        
        var output = await _savingsPlanRepository.GetAllByIncomeIdAsync(request.IncomeId, cancellationToken);
        return output;
    }
}