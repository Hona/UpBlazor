using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UpBlazor.Domain.Models;
using UpBlazor.Domain.Repositories;

namespace UpBlazor.Application.Features.Expenses;

public record DeleteExpenseCommand(Expense Expense) : IRequest;

public class DeleteExpenseCommandHandler : IRequestHandler<DeleteExpenseCommand>
{
    private readonly IExpenseRepository _expenseRepository;

    public DeleteExpenseCommandHandler(IExpenseRepository expenseRepository)
    {
        _expenseRepository = expenseRepository;
    }

    public async Task<Unit> Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
    {
        await _expenseRepository.DeleteAsync(request.Expense, cancellationToken);
        
        return Unit.Value;
    }
}
