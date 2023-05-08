using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UpBlazor.Domain.Entities;

namespace UpBlazor.Application.Repositories
{
    public interface IExpenseRepository : IGenericRepository<Expense>
    {
        Task<Expense> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Expense>> GetAllByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    }
}