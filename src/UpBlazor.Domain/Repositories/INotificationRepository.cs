using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UpBlazor.Domain.Models;

namespace UpBlazor.Domain.Repositories;

public interface INotificationRepository : IGenericRepository<Notification>
{
    Task<Notification> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}