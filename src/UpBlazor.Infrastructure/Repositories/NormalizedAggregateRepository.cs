using System.Threading;
using System.Threading.Tasks;
using Marten;
using UpBlazor.Application.Repositories;
using UpBlazor.Domain.Entities;
using UpBlazor.Domain.Entities.Normalized;

namespace UpBlazor.Infrastructure.Repositories
{
    public class NormalizedAggregateRepository : GenericRepository<NormalizedAggregate>, INormalizedAggregateRepository
    {
        public NormalizedAggregateRepository(IDocumentStore store) : base(store) { }

        public async Task<NormalizedAggregate> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            await using var session = Store.QuerySession();

            return await session.Query<NormalizedAggregate>()
                .SingleOrDefaultAsync(x => x.UserId == userId, cancellationToken);
        }
    }
}