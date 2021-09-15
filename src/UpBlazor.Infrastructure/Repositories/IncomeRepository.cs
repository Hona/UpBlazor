using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Marten;
using UpBlazor.Core.Models;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Infrastructure.Repositories
{
    public class IncomeRepository : GenericRepository<Income>, IIncomeRepository
    {
        public IncomeRepository(IDocumentStore store) : base(store) { }

        public async Task<Income> GetByIdAsync(Guid id) =>
            await Queryable
                .SingleOrDefaultAsync(x => x.Id == id);

        public async Task<IReadOnlyList<Income>> GetAllByUserIdAsync(string userId) =>
            await Queryable
                .Where(x => x.UserId == userId)
                .ToListAsync();
    }
}