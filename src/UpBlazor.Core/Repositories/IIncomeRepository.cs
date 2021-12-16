using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UpBlazor.Core.Models;

namespace UpBlazor.Core.Repositories
{
    public interface IIncomeRepository : IGenericRepository<Income>
    {
        Task<Income> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Income>> GetAllByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    }
}