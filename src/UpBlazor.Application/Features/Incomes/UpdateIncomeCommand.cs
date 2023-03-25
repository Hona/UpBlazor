using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UpBlazor.Application.Services;
using UpBlazor.Domain.Exceptions;
using UpBlazor.Domain.Models;
using UpBlazor.Domain.Repositories;

namespace UpBlazor.Application.Features.Incomes;

public record UpdateIncomeCommand(Income Income) : IRequest;

public class UpdateIncomeCommandHandler : IRequestHandler<UpdateIncomeCommand>
{
    private readonly IIncomeRepository _incomeRepository;
    private readonly ICurrentUserService _currentUserService;
    
    public UpdateIncomeCommandHandler(IIncomeRepository incomeRepository, ICurrentUserService currentUserService)
    {
        _incomeRepository = incomeRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(UpdateIncomeCommand request, CancellationToken cancellationToken)
    {
        var existingItem = await _incomeRepository.GetByIdAsync(request.Income.Id, cancellationToken);

        var userId = await _currentUserService.GetUserIdAsync(cancellationToken);
        if (existingItem.UserId != userId)
        {
            throw new BadRequestException("Income not found");
        }
        
        await _incomeRepository.UpdateAsync(request.Income, cancellationToken);
        
        return Unit.Value;
    }
}