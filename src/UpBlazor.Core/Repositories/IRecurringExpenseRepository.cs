using System;
using System.Threading.Tasks;
using UpBlazor.Core.Models;

namespace UpBlazor.Core.Repositories
{
    public interface IRecurringExpenseRepository : IGenericRepository<RecurringExpense>
    {
        Task<RecurringExpense> GetByIdAsync(Guid id);
    }
}