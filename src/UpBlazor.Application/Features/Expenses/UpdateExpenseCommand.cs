using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UpBlazor.Domain.Models;
using UpBlazor.Domain.Repositories;

namespace UpBlazor.Application.Features.Expenses;

public record UpdateExpenseCommand(Expense Expense) : IRequest;

public class UpdateExpenseCommandHandler : IRequestHandler<UpdateExpenseCommand>
{
    private readonly IExpenseRepository _expenseRepository;

    public UpdateExpenseCommandHandler(IExpenseRepository expenseRepository)
    {
        _expenseRepository = expenseRepository;
    }

    public async Task<Unit> Handle(UpdateExpenseCommand request, CancellationToken cancellationToken)
    {
        await _expenseRepository.UpdateAsync(request.Expense, cancellationToken);

        return Unit.Value;
    }
}