using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using UpBlazor.Core.Models;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Infrastructure.EfCore.Repositories
{
    internal class NormalizedAggregateRepository : GenericRepository<NormalizedAggregate>, INormalizedAggregateRepository
    {
        public NormalizedAggregateRepository(UpBankDbContext context) : base(context, context.NormalizedAggregates) { }

        public async Task<NormalizedAggregate> GetByUserIdAsync(string userId)
        {
            return await DbSet
                   .AsNoTracking()
                   .SingleOrDefaultAsync(x => x.UserId == userId);
        }
    }
}
