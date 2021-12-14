using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UpBlazor.Application.Services;
using UpBlazor.Core.Models;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Application.Features.Expenses;

public record CreateExpenseCommand(string Name, Money Amount, DateTime PaidByDate, Guid? FromIncomeId, string FromSaverId) : IRequest<Guid>;

public class CreateExpenseCommandHandler : IRequestHandler<CreateExpenseCommand, Guid>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IExpenseRepository _expenseRepository;

    public CreateExpenseCommandHandler(ICurrentUserService currentUserService, IExpenseRepository expenseRepository)
    {
        _currentUserService = currentUserService;
        _expenseRepository = expenseRepository;
    }

    public async Task<Guid> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
    {
        var userId = await _currentUserService.GetUserIdAsync();

        var output = new Expense
        {
            Name = request.Name,
            Money = request.Amount,
            PaidByDate = request.PaidByDate,
            FromIncomeId = request.FromIncomeId,
            FromSaverId = request.FromSaverId,
            UserId = userId
        };

        await _expenseRepository.AddAsync(output);

        return output.Id;
    }
}
