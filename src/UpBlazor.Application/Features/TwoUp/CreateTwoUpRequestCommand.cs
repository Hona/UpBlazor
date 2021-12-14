using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UpBlazor.Application.Services;
using UpBlazor.Core.Models;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Application.Features.TwoUp;

public record CreateTwoUpRequestCommand(string Email, string Message) : IRequest<string>;

public class CreateTwoUpRequestCommandHandler : IRequestHandler<CreateTwoUpRequestCommand, string>
{
    private readonly ITwoUpRequestRepository _twoUpRequestRepository;
    private readonly IRegisteredUserRepository _registeredUserRepository;
    private readonly ICurrentUserService _currentUserService;

    public CreateTwoUpRequestCommandHandler(ITwoUpRequestRepository twoUpRequestRepository, IRegisteredUserRepository registeredUserRepository, ICurrentUserService currentUserService)
    {
        _twoUpRequestRepository = twoUpRequestRepository;
        _registeredUserRepository = registeredUserRepository;
        _currentUserService = currentUserService;
    }

    public async Task<string> Handle(CreateTwoUpRequestCommand request, CancellationToken cancellationToken)
    {
        var userId = await _currentUserService.GetUserIdAsync(cancellationToken);
        
        var requestee = await _registeredUserRepository.GetByEmailAsync(request.Email, cancellationToken);

        var output = new TwoUpRequest
        {
            CreatedDate = DateTime.Now,
            RequesterId = userId,
            RequesterName = await _currentUserService.GetGivenNameAsync(cancellationToken),
            RequesterMessage = request.Message,
            RequesteeId = requestee.Id,
        };
        
        await _twoUpRequestRepository.AddAsync(output, cancellationToken);

        return output.MartenId;
    }
}