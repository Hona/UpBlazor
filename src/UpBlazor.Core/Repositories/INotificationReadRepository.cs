using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UpBlazor.Core.Models;

namespace UpBlazor.Core.Repositories;

public interface INotificationReadRepository : IGenericRepository<NotificationRead>
{
    Task<IReadOnlyList<NotificationRead>> GetByUserIdAsync(string id);
    Task<IReadOnlyList<NotificationRead>> GetByNotificationIdAsync(Guid id);
    Task<NotificationRead> GetByUserAndNotificationIdAsync(string userId, Guid notificationId);
}