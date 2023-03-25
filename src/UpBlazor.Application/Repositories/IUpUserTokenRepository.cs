using System.Threading;
using System.Threading.Tasks;
using UpBlazor.Domain.Entities;

namespace UpBlazor.Application.Repositories
{
    public interface IUpUserTokenRepository : IGenericRepository<UpUserToken>
    {
        Task<UpUserToken> GetByUserIdAsync(string id, CancellationToken cancellationToken = default);
    }
}