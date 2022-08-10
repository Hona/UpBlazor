using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UpBlazor.Core.Models;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Application.Features.Users;

public record RegisterUserCommand(RegisteredUser User) : IRequest;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand>
{
    private readonly IRegisteredUserRepository _registeredUserRepository;

    public RegisterUserCommandHandler(IRegisteredUserRepository registeredUserRepository)
    {
        _registeredUserRepository = registeredUserRepository;
    }

    public async Task<Unit> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        await _registeredUserRepository.AddOrUpdateAsync(request.User, cancellationToken);
        
        return Unit.Value;
    }
}