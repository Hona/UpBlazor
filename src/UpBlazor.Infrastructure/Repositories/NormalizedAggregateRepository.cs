using System.Threading.Tasks;
using Marten;
using Marten.Linq.SoftDeletes;
using UpBlazor.Core.Models;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Infrastructure.Repositories
{
    public class NormalizedAggregateRepository : GenericRepository<NormalizedAggregate>, INormalizedAggregateRepository
    {
        public NormalizedAggregateRepository(IDocumentStore store) : base(store) { }

        public async Task<NormalizedAggregate> GetByUserIdAsync(string userId)
        {
            using var session = Store.QuerySession();

            return await session.Query<NormalizedAggregate>()
                .SingleOrDefaultAsync(x => x.UserId == userId);
        }
    }
}