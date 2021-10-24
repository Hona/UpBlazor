using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UpBlazor.Core.Models;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Infrastructure.EfCore.Repositories
{
    internal class TwoUpRepository : GenericRepository<TwoUp>, ITwoUpRepository
    {
        public TwoUpRepository(UpBankDbContext context) : base(context, context.TwoUps) { }

        public async Task<IReadOnlyList<TwoUp>> GetAllByUserIdAsync(string userId)
        {
            return await DbSet
                .AsNoTracking()
                .Where(x => x.UserId1 == userId|| x.UserId2 == userId)
                .ToListAsync();
        }

        public async Task<TwoUp> GetByBothUserIdsAsync(string userId1, string userId2)
        {
            return await DbSet
                .AsNoTracking()
                .SingleOrDefaultAsync(x => (x.UserId1 == userId1 && x.UserId2 == userId2)
                         || (x.UserId1 == userId2 && x.UserId2 == userId1));
        }
    }
}
