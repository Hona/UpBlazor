﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UpBlazor.Application.Repositories;
using UpBlazor.Domain.Entities;

namespace UpBlazor.Application.Features.SavingsPlans;

public record DeleteSavingsPlanCommand(SavingsPlan SavingsPlan) : IRequest;

public class DeleteSavingsPlanCommandHandler : IRequestHandler<DeleteSavingsPlanCommand>
{
    private readonly ISavingsPlanRepository _savingsPlanRepository;

    public DeleteSavingsPlanCommandHandler(ISavingsPlanRepository savingsPlanRepository)
    {
        _savingsPlanRepository = savingsPlanRepository;
    }

    public async Task<Unit> Handle(DeleteSavingsPlanCommand request, CancellationToken cancellationToken)
    {
        await _savingsPlanRepository.DeleteAsync(request.SavingsPlan, cancellationToken);
        
        return Unit.Value;
    }
}