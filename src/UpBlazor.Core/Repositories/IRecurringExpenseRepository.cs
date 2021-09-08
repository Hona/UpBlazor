using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UpBlazor.Core.Models;

namespace UpBlazor.Core.Repositories
{
    public interface IRecurringExpenseRepository : IGenericRepository<RecurringExpense>
    {
        Task<RecurringExpense> GetByIdAsync(Guid id);
        Task<IReadOnlyList<RecurringExpense>> GetAllByUserIdAsync(string userId);
    }
}