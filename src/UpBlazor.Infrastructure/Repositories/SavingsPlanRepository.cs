using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Marten;
using UpBlazor.Core.Models;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Infrastructure.Repositories
{
    public class SavingsPlanRepository : GenericRepository<SavingsPlan>, ISavingsPlanRepository
    {
        public SavingsPlanRepository(IDocumentStore store) : base(store) { }

        public async Task<SavingsPlan> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await using var session = Store.QuerySession();

            return await session.Query<SavingsPlan>()
                .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<IReadOnlyList<SavingsPlan>> GetAllByIncomeIdAsync(Guid incomeId, CancellationToken cancellationToken = default)
        {
            await using var session = Store.QuerySession();

            return await session.Query<SavingsPlan>()
                .Where(x => x.IncomeId == incomeId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<SavingsPlan>> GetAllByUserIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            await using var session = Store.QuerySession();

            var userIncomeIds = (await session.Query<Income>()
                .Where(x => x.UserId == userId)
                .Select(x => x.Id)
                .ToListAsync(cancellationToken)).ToArray();

            return await session.Query<SavingsPlan>()
                .Where(x => x.IncomeId.IsOneOf(userIncomeIds))
                .ToListAsync(cancellationToken);        
        }
    }
}