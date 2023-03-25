using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UpBlazor.Domain.Models;

namespace UpBlazor.Domain.Repositories
{
    public interface ISavingsPlanRepository : IGenericRepository<SavingsPlan>
    {
        Task<SavingsPlan> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<SavingsPlan>> GetAllByIncomeIdAsync(Guid incomeId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<SavingsPlan>> GetAllByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    }
}