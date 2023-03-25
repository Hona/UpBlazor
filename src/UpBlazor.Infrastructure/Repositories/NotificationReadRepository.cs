using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Marten;
using UpBlazor.Application.Repositories;
using UpBlazor.Domain.Entities;

namespace UpBlazor.Infrastructure.Repositories;

public class NotificationReadRepository : GenericRepository<NotificationRead>, INotificationReadRepository
{
    public NotificationReadRepository(IDocumentStore store) : base(store) { }

    public async Task<IReadOnlyList<NotificationRead>> GetByUserIdAsync(string id, CancellationToken cancellationToken = default)
    {
        await using var session = Store.QuerySession();

        return await session.Query<NotificationRead>()
            .Where(x => x.UserId == id)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<NotificationRead>> GetByNotificationIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await using var session = Store.QuerySession();

        return await session.Query<NotificationRead>()
            .Where(x => x.NotificationId == id)
            .ToListAsync(cancellationToken);
        
    }

    public async Task<NotificationRead> GetByUserAndNotificationIdAsync(string userId, Guid notificationId, CancellationToken cancellationToken = default)
    {
        await using var session = Store.QuerySession();

        return await session.Query<NotificationRead>()
            .SingleOrDefaultAsync(x => x.UserId == userId && x.NotificationId == notificationId, cancellationToken);
    }
}