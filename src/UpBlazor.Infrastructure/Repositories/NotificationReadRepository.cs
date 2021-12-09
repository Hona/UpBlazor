using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Marten;
using UpBlazor.Core.Models;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Infrastructure.Repositories;

public class NotificationReadRepository : GenericRepository<NotificationRead>, INotificationReadRepository
{
    public NotificationReadRepository(IDocumentStore store) : base(store) { }

    public async Task<IReadOnlyList<NotificationRead>> GetByUserIdAsync(string id)
    {
        await using var session = Store.QuerySession();

        return await session.Query<NotificationRead>()
            .Where(x => x.UserId == id)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<NotificationRead>> GetByNotificationIdAsync(Guid id)
    {
        await using var session = Store.QuerySession();

        return await session.Query<NotificationRead>()
            .Where(x => x.NotificationId == id)
            .ToListAsync();    }

    public async Task<NotificationRead> GetByUserAndNotificationIdAsync(string userId, Guid notificationId)
    {
        await using var session = Store.QuerySession();

        return await session.Query<NotificationRead>()
            .SingleOrDefaultAsync(x => x.UserId == userId && x.NotificationId == notificationId);
    }
}