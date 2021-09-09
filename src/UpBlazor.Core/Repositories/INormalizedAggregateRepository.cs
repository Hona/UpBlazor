using System.Collections.Generic;
using System.Threading.Tasks;
using UpBlazor.Core.Models;

namespace UpBlazor.Core.Repositories
{
    public interface INormalizedAggregateRepository : IGenericRepository<NormalizedAggregate>
    {
        Task<NormalizedAggregate> GetByUserIdAsync(string userId);
    }
}