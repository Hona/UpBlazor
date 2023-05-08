﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UpBlazor.Application.Common.Services;
using UpBlazor.Application.Repositories;
using UpBlazor.Domain.Events;

namespace UpBlazor.Application.Features.Normalized.EventHandlers;

public class SavingsPlanUpdatedHandler : INotificationHandler<SavingsPlanUpdatedEvent>
{
    private readonly INormalizerService _normalizerService;
    private readonly IIncomeRepository _incomeRepository;

    public SavingsPlanUpdatedHandler(INormalizerService normalizerService, IIncomeRepository incomeRepository)
    {
        _normalizerService = normalizerService;
        _incomeRepository = incomeRepository;
    }

    public async Task Handle(SavingsPlanUpdatedEvent notification, CancellationToken cancellationToken)
    {
        var income = await _incomeRepository.GetByIdAsync(notification.SavingsPlan.IncomeId, cancellationToken);
        await _normalizerService.UpdateUserAsync(income.UserId, cancellationToken);
    }
}