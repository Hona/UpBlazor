using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace UpBlazor.Domain.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task AddAsync(T model, CancellationToken cancellationToken = default);
        Task UpdateAsync(T model, CancellationToken cancellationToken = default);
        Task AddOrUpdateAsync(T model, CancellationToken cancellationToken = default);
        Task DeleteAsync(T model, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}