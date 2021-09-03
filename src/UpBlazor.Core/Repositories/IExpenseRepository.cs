using System;
using System.Threading.Tasks;
using UpBlazor.Core.Models;

namespace UpBlazor.Core.Repositories
{
    public interface IExpenseRepository : IGenericRepository<Expense>
    {
        Task<Expense> GetByIdAsync(Guid id);
    }
}