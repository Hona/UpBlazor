using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Marten;
using UpBlazor.Core.Models;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Infrastructure.Repositories;

public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
{
    public NotificationRepository(IDocumentStore store) : base(store) { }

    public async Task<Notification> GetByIdAsync(Guid id)
    {
        await using var session = Store.QuerySession();

        return await session.Query<Notification>()
            .SingleOrDefaultAsync(x => x.Id == id);
    }
}