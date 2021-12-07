using System.Threading.Tasks;
using Marten;
using UpBlazor.Core.Models;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Infrastructure.Repositories
{
    public class NormalizedAggregateRepository : GenericRepository<NormalizedAggregate>, INormalizedAggregateRepository
    {
        public NormalizedAggregateRepository(IDocumentStore store) : base(store) { }

        public async Task<NormalizedAggregate> GetByUserIdAsync(string userId)
        {
            var session = Store.QuerySession();
            await using var _ = session.ConfigureAwait(false);

            return await session.Query<NormalizedAggregate>()
                .SingleOrDefaultAsync(x => x.UserId == userId).ConfigureAwait(false);
        }
    }
}