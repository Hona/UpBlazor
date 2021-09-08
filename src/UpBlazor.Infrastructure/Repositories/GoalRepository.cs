using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Marten;
using UpBlazor.Core;
using UpBlazor.Core.Models;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Infrastructure.Repositories
{
    public class GoalRepository : GenericRepository<Goal>, IGoalRepository
    {
        public GoalRepository(IDocumentStore store) : base(store) { }

        public async Task<Goal> GetByIdAsync(Guid id)
        {
            using var session = Store.QuerySession();

            return await session.Query<Goal>()
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IReadOnlyList<Goal>> GetAllByUserIdAsync(string userId)
        {
            using var session = Store.QuerySession();

            return await session.Query<Goal>()
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }
    }
}