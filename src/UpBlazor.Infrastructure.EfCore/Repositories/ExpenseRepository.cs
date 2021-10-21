using Microsoft.EntityFrameworkCore;
using UpBlazor.Core.Models;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Infrastructure.EfCore.Repositories
{
    internal class ExpenseRepository : GenericRepository<Expense>, IExpenseRepository
    {
        public ExpenseRepository(UpBankDbContext context) : base(context, context.Expenses) { }

        public async Task<IReadOnlyList<Expense>> GetAllByUserIdAsync(string userId)
        {
            return await DbSet
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }

        public Task<Expense> GetByIdAsync(Guid id)
        {
            return DbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
