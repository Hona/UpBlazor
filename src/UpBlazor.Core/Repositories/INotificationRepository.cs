using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UpBlazor.Core.Models;

namespace UpBlazor.Core.Repositories;

public interface INotificationRepository : IGenericRepository<Notification>
{
    Task<Notification> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}