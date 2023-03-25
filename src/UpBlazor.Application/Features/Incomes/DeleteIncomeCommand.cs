using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UpBlazor.Domain.Models;
using UpBlazor.Domain.Repositories;

namespace UpBlazor.Application.Features.Incomes;

public record DeleteIncomeCommand(Income Income) : IRequest;

public class DeleteIncomeCommandHandler : IRequestHandler<DeleteIncomeCommand>
{
    private readonly IIncomeRepository _incomeRepository;

    public DeleteIncomeCommandHandler(IIncomeRepository incomeRepository)
    {
        _incomeRepository = incomeRepository;
    }

    public async Task<Unit> Handle(DeleteIncomeCommand request, CancellationToken cancellationToken)
    {
        await _incomeRepository.DeleteAsync(request.Income, cancellationToken);

        return Unit.Value;
    }
}
