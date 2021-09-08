using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UpBlazor.Core.Models;

namespace UpBlazor.Core.Repositories
{
    public interface IGoalRepository : IGenericRepository<Goal>
    {
        Task<Goal> GetByIdAsync(Guid id);
        Task<IReadOnlyList<Goal>> GetAllByUserIdAsync(string userId);
    }
}