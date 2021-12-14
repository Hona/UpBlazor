using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UpBlazor.Core.Models;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Application.Features.RecurringExpenses;

public record DeleteRecurringExpenseCommand(RecurringExpense RecurringExpense) : IRequest;

public class DeleteRecurringExpenseCommandHandler : IRequestHandler<DeleteRecurringExpenseCommand>
{
    private readonly IRecurringExpenseRepository _recurringExpenseRepository;

    public DeleteRecurringExpenseCommandHandler(IRecurringExpenseRepository recurringExpenseRepository)
    {
        _recurringExpenseRepository = recurringExpenseRepository;
    }

    public async Task<Unit> Handle(DeleteRecurringExpenseCommand request, CancellationToken cancellationToken)
    {
        await _recurringExpenseRepository.DeleteAsync(request.RecurringExpense);
        
        return Unit.Value;
    }
}
