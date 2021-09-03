using System;
using System.Threading.Tasks;
using Marten;
using UpBlazor.Core.Models;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Infrastructure.Repositories
{
    public class RecurringExpenseRepository : GenericRepository<RecurringExpense>, IRecurringExpenseRepository
    {
        public RecurringExpenseRepository(IDocumentStore store) : base(store) { }

        public async Task<RecurringExpense> GetByIdAsync(Guid id)
        {
            using var session = Store.QuerySession();

            return await session.Query<RecurringExpense>()
                .SingleOrDefaultAsync(x => x.Id == id);
        }
    }
}