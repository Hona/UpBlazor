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

        public async Task<SavingsPlan> GetByIdAsync(Guid id)
        {
            var session = Store.QuerySession();
            await using var _ = session.ConfigureAwait(false);

            return await session.Query<SavingsPlan>()
                .SingleOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<SavingsPlan>> GetAllByIncomeIdAsync(Guid incomeId)
        {
            var session = Store.QuerySession();
            await using var _ = session.ConfigureAwait(false);

            return await session.Query<SavingsPlan>()
                .Where(x => x.IncomeId == incomeId)
                .ToListAsync().ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<SavingsPlan>> GetAllByUserIdAsync(string userId)
        {
            var session = Store.QuerySession();
            await using var _ = session.ConfigureAwait(false);

            var userIncomeIds = (await session.Query<Income>()
                .Where(x => x.UserId == userId)
                .Select(x => x.Id)
                .ToListAsync().ConfigureAwait(false)).ToArray();

            return await session.Query<SavingsPlan>()
                .Where(x => x.IncomeId.IsOneOf(userIncomeIds))
                .ToListAsync().ConfigureAwait(false);        
        }
    }
}