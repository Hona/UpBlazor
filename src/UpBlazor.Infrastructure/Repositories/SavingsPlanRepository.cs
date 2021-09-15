using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Marten;
using UpBlazor.Core.Models;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Infrastructure.Repositories
{
    public class SavingsPlanRepository : GenericRepository<SavingsPlan>, ISavingsPlanRepository
    {
        public SavingsPlanRepository(IDocumentStore store) : base(store) { }

        public async Task<SavingsPlan> GetByIdAsync(Guid id) =>
            await Queryable
                .SingleOrDefaultAsync(x => x.Id == id);

        public async Task<IReadOnlyList<SavingsPlan>> GetAllByIncomeIdAsync(Guid incomeId) =>
            await Queryable
                .Where(x => x.IncomeId == incomeId)
                .ToListAsync();

        public async Task<IReadOnlyList<SavingsPlan>> GetAllByUserIdAsync(string userId)
        {
            var userIncomeIds = (await Session.Query<Income>()
                .Where(x => x.UserId == userId)
                .Select(x => x.Id)
                .ToListAsync()).ToArray();

            return await Queryable
                .Where(x => x.IncomeId.IsOneOf(userIncomeIds))
                .ToListAsync();        
        }
    }
}