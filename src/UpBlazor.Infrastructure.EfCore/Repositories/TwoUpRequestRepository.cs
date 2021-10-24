using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UpBlazor.Core.Models;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Infrastructure.EfCore.Repositories
{
    internal class TwoUpRequestRepository : GenericRepository<TwoUpRequest>, ITwoUpRequestRepository
    {
        public TwoUpRequestRepository(UpBankDbContext context) : base(context, context.TwoUpRequests) { }

        public Task<TwoUpRequest> GetByRequesterAndRequesteeAsync(string requesterId, string requesteeId)
        {
            return DbSet
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.RequesterId == requesteeId
                                        && x.RequesteeId == requesteeId);
        }

        public async Task<IReadOnlyList<TwoUpRequest>> GetAllByRequesterAsync(string requesterId)
        {
            return await DbSet
                .AsNoTracking()
                .Where(x => x.RequesterId == requesterId)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<TwoUpRequest>> GetAllByRequesteeAsync(string requesteeId)
        {
            return await DbSet
                .AsNoTracking()
                .Where(x => x.RequesteeId == requesteeId)
                .ToListAsync();
        }
    }
}
