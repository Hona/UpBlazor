using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UpBlazor.Core.Models;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Infrastructure.EfCore.Repositories
{
    internal class IncomeRepository : GenericRepository<Income>, IIncomeRepository
    {
        public IncomeRepository(UpBankDbContext context) : base(context, context.Incomes) { }

        public async Task<Income> GetByIdAsync(Guid id)
        {
            return await DbSet
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IReadOnlyList<Income>> GetAllByUserIdAsync(string userId)
        {
            return await DbSet
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }
    }
}
