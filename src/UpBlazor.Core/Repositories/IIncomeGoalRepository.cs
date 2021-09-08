using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UpBlazor.Core.Models;

namespace UpBlazor.Core.Repositories
{
    public interface IIncomeGoalRepository : IGenericRepository<IncomeGoal>
    {
        Task<IncomeGoal> GetByIdAsync(Guid id);
        Task<IReadOnlyList<IncomeGoal>> GetAllByIncomeIdAsync(Guid incomeId);
        Task<IReadOnlyList<IncomeGoal>> GetAllByUserIdAsync(string userId);
    }
}