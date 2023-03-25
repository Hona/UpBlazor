using System;
using System.Threading;
using System.Threading.Tasks;
using UpBlazor.Domain.Entities;

namespace UpBlazor.Application.Repositories;

public interface INotificationRepository : IGenericRepository<Notification>
{
    Task<Notification> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}