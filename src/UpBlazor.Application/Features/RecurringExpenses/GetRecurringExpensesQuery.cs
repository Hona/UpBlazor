using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UpBlazor.Application.Services;
using UpBlazor.Core.Models;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Application.Features.RecurringExpenses;

public record GetRecurringExpensesQuery : IRequest<IReadOnlyList<RecurringExpense>>;

public class GetRecurringExpensesQueryHandler : IRequestHandler<GetRecurringExpensesQuery, IReadOnlyList<RecurringExpense>>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IRecurringExpenseRepository _recurringExpenseRepository;

    public GetRecurringExpensesQueryHandler(ICurrentUserService currentUserService, IRecurringExpenseRepository recurringExpenseRepository)
    {
        _currentUserService = currentUserService;
        _recurringExpenseRepository = recurringExpenseRepository;
    }

    public async Task<IReadOnlyList<RecurringExpense>> Handle(GetRecurringExpensesQuery request, CancellationToken cancellationToken)
    {
        var userId = await _currentUserService.GetUserIdAsync();

        var output = await _recurringExpenseRepository.GetAllByUserIdAsync(userId);
        return output;
    }
}
