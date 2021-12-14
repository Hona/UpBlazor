using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UpBlazor.Core.Models;

namespace UpBlazor.Core.Repositories
{
    public interface ISavingsPlanRepository : IGenericRepository<SavingsPlan>
    {
        Task<SavingsPlan> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<SavingsPlan>> GetAllByIncomeIdAsync(Guid incomeId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<SavingsPlan>> GetAllByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    }
}