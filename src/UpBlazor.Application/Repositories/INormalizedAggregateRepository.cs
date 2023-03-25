using System.Threading;
using System.Threading.Tasks;
using UpBlazor.Domain.Entities;
using UpBlazor.Domain.Entities.Normalized;

namespace UpBlazor.Application.Repositories
{
    public interface INormalizedAggregateRepository : IGenericRepository<NormalizedAggregate>
    {
        Task<NormalizedAggregate> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    }
}