using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UpBlazor.Application.Services;
using UpBlazor.Domain.Models;
using UpBlazor.Domain.Repositories;

namespace UpBlazor.Application.Features.Incomes;

public record GetIncomesQuery : IRequest<IReadOnlyList<Income>>;

public class GetIncomesQueryHandler : IRequestHandler<GetIncomesQuery, IReadOnlyList<Income>>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IIncomeRepository _incomeRepository;

    public GetIncomesQueryHandler(ICurrentUserService currentUserService, IIncomeRepository incomeRepository)
    {
        _currentUserService = currentUserService;
        _incomeRepository = incomeRepository;
    }

    public async Task<IReadOnlyList<Income>> Handle(GetIncomesQuery request, CancellationToken cancellationToken)
    {
        var userId = await _currentUserService.GetUserIdAsync(cancellationToken);

        var output = await _incomeRepository.GetAllByUserIdAsync(userId, cancellationToken);
        return output;
    }
}