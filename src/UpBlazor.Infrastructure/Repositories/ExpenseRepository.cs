using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Marten;
using UpBlazor.Application.Repositories;
using UpBlazor.Domain.Entities;

namespace UpBlazor.Infrastructure.Repositories
{
    public class ExpenseRepository : GenericRepository<Expense>, IExpenseRepository
    {
        public ExpenseRepository(IDocumentStore store) : base(store) { }

        public async Task<Expense> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await using var session = Store.QuerySession();

            return await session.Query<Expense>()
                .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<IReadOnlyList<Expense>> GetAllByUserIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            await using var session = Store.QuerySession();

            return await session.Query<Expense>()
                .Where(x => x.UserId == userId)
                .ToListAsync(cancellationToken);
        }
    }
}