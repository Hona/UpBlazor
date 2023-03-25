using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UpBlazor.Application.Services;
using UpBlazor.Domain.Models;
using UpBlazor.Domain.Models.Enums;
using UpBlazor.Domain.Repositories;

namespace UpBlazor.Application.Features.RecurringExpenses;

public record CreateRecurringExpenseCommand(string Name, Interval Interval, int IntervalUnits, Money Amount, DateTime StartDate, string FromSaverId) : IRequest<Guid>;

public class CreateRecurringExpenseCommandHandler : IRequestHandler<CreateRecurringExpenseCommand, Guid>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IRecurringExpenseRepository _recurringExpenseRepository;

    public CreateRecurringExpenseCommandHandler(IRecurringExpenseRepository recurringExpenseRepository, ICurrentUserService currentUserService)
    {
        _recurringExpenseRepository = recurringExpenseRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Guid> Handle(CreateRecurringExpenseCommand request, CancellationToken cancellationToken)
    {
        var userId = await _currentUserService.GetUserIdAsync(cancellationToken);

        var output = new RecurringExpense
        {
            Name = request.Name,
            Interval = request.Interval,
            IntervalUnits = request.IntervalUnits,
            Money = request.Amount,
            StartDate = request.StartDate,
            FromSaverId = request.FromSaverId,
            UserId = userId
        };

        await _recurringExpenseRepository.AddAsync(output, cancellationToken);

        return output.Id;
    }
}
