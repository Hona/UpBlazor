using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Marten;
using UpBlazor.Core.Models;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Infrastructure.Repositories
{
    public class IncomeGoalRepository : GenericRepository<IncomeGoal>, IIncomeGoalRepository
    {
        public IncomeGoalRepository(IDocumentStore store) : base(store) { }

        public async Task<IncomeGoal> GetByIdAsync(Guid id)
        {
            using var session = Store.QuerySession();

            return await session.Query<IncomeGoal>()
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IReadOnlyList<IncomeGoal>> GetAllByIncomeIdAsync(Guid incomeId)
        {
            using var session = Store.QuerySession();

            return await session.Query<IncomeGoal>()
                .Where(x => x.IncomeId == incomeId)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<IncomeGoal>> GetAllByUserIdAsync(string userId)
        {
            using var session = Store.QuerySession();

            var userIncomeIds = await session.Query<Income>()
                .Where(x => x.UserId == userId)
                .Select(x => x.Id)
                .ToListAsync();

            return await session.Query<IncomeGoal>()
                .Where(x => userIncomeIds.Contains(x.IncomeId))
                .ToListAsync();        
        }
    }
}