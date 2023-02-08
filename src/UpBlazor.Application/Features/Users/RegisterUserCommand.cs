using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UpBlazor.Application.Services;
using UpBlazor.Core.Models;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Application.Features.Users;

public record RegisterUserCommand() : IRequest;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand>
{
    private readonly IRegisteredUserRepository _registeredUserRepository;
    private readonly ICurrentUserService _currentUserService;

    public RegisterUserCommandHandler(IRegisteredUserRepository registeredUserRepository, ICurrentUserService currentUserService)
    {
        _registeredUserRepository = registeredUserRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.IsImpersonating())
        {
            return Unit.Value;
        }

        var claims = await _currentUserService.GetClaimsAsync(cancellationToken);
        
        var registeredUser = new RegisteredUser
        {
            Id = await _currentUserService.GetUserIdAsync(cancellationToken),
            Email = claims.FirstOrDefault(x => x.Type == "email").Value,
            GivenName = claims.FirstOrDefault(x => x.Type == "given_name").Value
        };

        await _registeredUserRepository.AddOrUpdateAsync(registeredUser, cancellationToken);
        
        return Unit.Value;
    }
}