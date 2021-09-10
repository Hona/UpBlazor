using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UpBlazor.Core.Models;

namespace UpBlazor.Core.Repositories
{
    public interface ISavingsPlanRepository : IGenericRepository<SavingsPlan>
    {
        Task<SavingsPlan> GetByIdAsync(Guid id);
        Task<IReadOnlyList<SavingsPlan>> GetAllByIncomeIdAsync(Guid incomeId);
        Task<IReadOnlyList<SavingsPlan>> GetAllByUserIdAsync(string userId);
    }
}