using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UpBlazor.Core.Models;

namespace UpBlazor.Core.Repositories
{
    public interface IGoalRepository : IGenericRepository<Goal>
    {
        Task<Goal> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Goal>> GetAllByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    }
}