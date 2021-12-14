using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UpBlazor.Core.Models;

namespace UpBlazor.Core.Repositories
{
    public interface ITwoUpRequestRepository : IGenericRepository<TwoUpRequest>
    {
        Task<TwoUpRequest> GetByRequesterAndRequesteeAsync(string requesterId, string requesteeId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<TwoUpRequest>> GetAllByRequesterAsync(string requesterId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<TwoUpRequest>> GetAllByRequesteeAsync(string requesteeId, CancellationToken cancellationToken = default);
    }
}