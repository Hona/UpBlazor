using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UpBlazor.Core.Models;

namespace UpBlazor.Core.Repositories
{
    public interface ITwoUpRepository : IGenericRepository<TwoUp>
    {
        Task<IReadOnlyList<TwoUp>> GetAllByUserIdAsync(string userId, CancellationToken cancellationToken = default);
        Task<TwoUp> GetByBothUserIdsAsync(string userId1, string userId2, CancellationToken cancellationToken = default);
    }
}