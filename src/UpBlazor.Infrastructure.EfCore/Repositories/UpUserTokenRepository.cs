using Microsoft.EntityFrameworkCore;
using UpBlazor.Core.Models;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Infrastructure.EfCore.Repositories
{
    internal class UpUserTokenRepository : GenericRepository<UpUserToken>, IUpUserTokenRepository
    {
        public UpUserTokenRepository(UpBankDbContext context) : base(context, context.UpUserTokens) { }

        public Task<UpUserToken> GetByUserIdAsync(string id)
        {
            return DbSet
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.UserId == id);
        }
    }
}
