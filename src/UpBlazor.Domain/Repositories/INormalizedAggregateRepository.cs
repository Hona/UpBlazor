using System.Threading;
using System.Threading.Tasks;
using UpBlazor.Domain.Models;

namespace UpBlazor.Domain.Repositories
{
    public interface INormalizedAggregateRepository : IGenericRepository<NormalizedAggregate>
    {
        Task<NormalizedAggregate> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    }
}