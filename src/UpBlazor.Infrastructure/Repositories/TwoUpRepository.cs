using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Marten;
using UpBlazor.Core.Models;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Infrastructure.Repositories
{
    public class TwoUpRepository : GenericRepository<TwoUp>, ITwoUpRepository
    {
        public TwoUpRepository(IDocumentStore store) : base(store) { }
        
        public async Task<IReadOnlyList<TwoUp>> GetAllByUserIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            await using var session = Store.QuerySession();

            return await session.Query<TwoUp>()
                .Where(x => x.UserId1 == userId || x.UserId2 == userId)
                .ToListAsync(cancellationToken);
        }

        public async Task<TwoUp> GetByBothUserIdsAsync(string userId1, string userId2, CancellationToken cancellationToken = default)
        {
            await using var session = Store.QuerySession();

            return await session.Query<TwoUp>()
                .SingleOrDefaultAsync(x => x.UserId1 == userId1 && x.UserId2 == userId2 ||
                                           x.UserId1 == userId2 && x.UserId2 == userId1, cancellationToken);
        }
    }
}