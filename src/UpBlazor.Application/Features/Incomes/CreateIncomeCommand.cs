using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UpBlazor.Application.Services;
using UpBlazor.Domain.Models;
using UpBlazor.Domain.Models.Enums;
using UpBlazor.Domain.Repositories;

namespace UpBlazor.Application.Features.Incomes;

public record CreateIncomeCommand(string Name, DateTime StartDate, decimal ExactMoney, Interval Interval, int IntervalUnits) : IRequest<Guid>;

public class CreateIncomeCommandHandler : IRequestHandler<CreateIncomeCommand, Guid>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IIncomeRepository _incomeRepository;

    public CreateIncomeCommandHandler(ICurrentUserService currentUserService, IIncomeRepository incomeRepository)
    {
        _currentUserService = currentUserService;
        _incomeRepository = incomeRepository;
    }

    public async Task<Guid> Handle(CreateIncomeCommand request, CancellationToken cancellationToken)
    {
        var userId = await _currentUserService.GetUserIdAsync(cancellationToken);

        var output = new Income
        {
            Name = request.Name,
            Interval = request.Interval,
            IntervalUnits = request.IntervalUnits,
            ExactMoney = request.ExactMoney,
            StartDate = request.StartDate,
            UserId = userId
        };
        
        await _incomeRepository.AddAsync(output, cancellationToken);

        return output.Id;
    }
}
