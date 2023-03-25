using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UpBlazor.Domain.Models;
using UpBlazor.Domain.Repositories;

namespace UpBlazor.Application.Features.RecurringExpenses;

public record UpdateRecurringExpenseCommand(RecurringExpense RecurringExpense) : IRequest;

public class UpdateRecurringExpenseCommandHandler : IRequestHandler<UpdateRecurringExpenseCommand>
{
    private readonly IRecurringExpenseRepository _recurringExpenseRepository;

    public UpdateRecurringExpenseCommandHandler(IRecurringExpenseRepository recurringExpenseRepository)
    {
        _recurringExpenseRepository = recurringExpenseRepository;
    }

    public async Task<Unit> Handle(UpdateRecurringExpenseCommand request, CancellationToken cancellationToken)
    {
        await _recurringExpenseRepository.UpdateAsync(request.RecurringExpense, cancellationToken);

        return Unit.Value;
    }
}