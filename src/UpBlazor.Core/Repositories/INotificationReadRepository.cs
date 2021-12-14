using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UpBlazor.Core.Models;

namespace UpBlazor.Core.Repositories;

public interface INotificationReadRepository : IGenericRepository<NotificationRead>
{
    Task<IReadOnlyList<NotificationRead>> GetByUserIdAsync(string id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<NotificationRead>> GetByNotificationIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<NotificationRead> GetByUserAndNotificationIdAsync(string userId, Guid notificationId, CancellationToken cancellationToken = default);
}