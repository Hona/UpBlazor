using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UpBlazor.Application.Services;
using UpBlazor.Core.Models;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Application.Features.Notifications;

public record CreateNotificationCommand(string Title, string Description) : IRequest<Guid>;

public class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand, Guid>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IRegisteredUserRepository _registeredUserRepository;
    
    public CreateNotificationCommandHandler(INotificationRepository notificationRepository, ICurrentUserService currentUserService, IRegisteredUserRepository registeredUserRepository)
    {
        _notificationRepository = notificationRepository;
        _currentUserService = currentUserService;
        _registeredUserRepository = registeredUserRepository;
    }

    public async Task<Guid> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
    {
        var userId = await _currentUserService.GetUserIdAsync(cancellationToken);
        var user = await _registeredUserRepository.GetByIdAsync(userId, cancellationToken);

        var output = new Notification
        {
            Title = request.Title,
            Description = request.Description,
            CreatedAt = DateTime.Now,
            Author = user.GivenName
        };
        
        await _notificationRepository.AddAsync(output, cancellationToken);

        return output.Id;
    }
}