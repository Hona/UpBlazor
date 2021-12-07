using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Marten;
using UpBlazor.Core.Models;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Infrastructure.Repositories
{
    public class TwoUpRequestRepository : GenericRepository<TwoUpRequest>, ITwoUpRequestRepository
    {
        public TwoUpRequestRepository(IDocumentStore store) : base(store) { }

        public async Task<TwoUpRequest> GetByRequesterAndRequesteeAsync(string requesterId, string requesteeId)
        {
            var session = Store.QuerySession();
            await using var _ = session.ConfigureAwait(false);

            return await session.Query<TwoUpRequest>()
                .SingleOrDefaultAsync(x => x.RequesterId == requesterId &&
                                           x.RequesteeId == requesteeId).ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<TwoUpRequest>> GetAllByRequesterAsync(string requesterId)
        {
            var session = Store.QuerySession();
            await using var _ = session.ConfigureAwait(false);

            return await session.Query<TwoUpRequest>()
                .Where(x => x.RequesterId == requesterId)
                .ToListAsync().ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<TwoUpRequest>> GetAllByRequesteeAsync(string requesteeId)
        {
            var session = Store.QuerySession();
            await using var _ = session.ConfigureAwait(false);

            return await session.Query<TwoUpRequest>()
                .Where(x => x.RequesteeId == requesteeId)
                .ToListAsync().ConfigureAwait(false);        
        }
    }
}