using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UpBlazor.Core.Models;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Infrastructure.EfCore.Repositories
{
    internal class SavingsPlanRepository : GenericRepository<SavingsPlan>, ISavingsPlanRepository
    {
        public SavingsPlanRepository(UpBankDbContext context) : base(context, context.SavingsPlans) { }

        public async Task<IReadOnlyList<SavingsPlan>> GetAllByIncomeIdAsync(Guid incomeId)
            => await DbSet
                .AsNoTracking()
                .Where(x => x.IncomeId == incomeId)
                .ToListAsync();

        public async Task<IReadOnlyList<SavingsPlan>> GetAllByUserIdAsync(string userId)
        {
            var userIncomeIds = (await Context.Incomes
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .Select(x => x.Id)
                .ToListAsync()).ToArray();

            return await DbSet
                .AsNoTracking()
                .Where(x => userIncomeIds.Contains(x.IncomeId))
                .ToListAsync();
        }

        public async Task<SavingsPlan> GetByIdAsync(Guid id)
            => await DbSet
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);
    }
}
