using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UpBlazor.Application.Services;
using UpBlazor.Core.Models;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Application.Features.Expenses;

public record GetExpensesQuery : IRequest<IReadOnlyList<Expense>>;

public class GetExpensesQueryHandler : IRequestHandler<GetExpensesQuery, IReadOnlyList<Expense>>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IExpenseRepository _expenseRepository;

    public GetExpensesQueryHandler(ICurrentUserService currentUserService, IExpenseRepository expenseRepository)
    {
        _currentUserService = currentUserService;
        _expenseRepository = expenseRepository;
    }

    public async Task<IReadOnlyList<Expense>> Handle(GetExpensesQuery request, CancellationToken cancellationToken)
    {
        var userId = await _currentUserService.GetUserIdAsync(cancellationToken);

        var output = await _expenseRepository.GetAllByUserIdAsync(userId, cancellationToken);
        return output;
    }
}
