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

        public async Task<TwoUpRequest> GetByRequesterAndRequesteeAsync(string requesterId, string requesteeId) =>
            await Queryable
                .SingleOrDefaultAsync(x => x.RequesterId == requesterId &&
                                           x.RequesteeId == requesteeId);

        public async Task<IReadOnlyList<TwoUpRequest>> GetAllByRequesterAsync(string requesterId) =>
            await Queryable
                .Where(x => x.RequesterId == requesterId)
                .ToListAsync();

        public async Task<IReadOnlyList<TwoUpRequest>> GetAllByRequesteeAsync(string requesteeId) =>
            await Queryable
                .Where(x => x.RequesteeId == requesteeId)
                .ToListAsync();
    }
}