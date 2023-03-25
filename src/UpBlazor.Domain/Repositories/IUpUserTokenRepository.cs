using System.Threading;
using System.Threading.Tasks;
using UpBlazor.Domain.Models;

namespace UpBlazor.Domain.Repositories
{
    public interface IUpUserTokenRepository : IGenericRepository<UpUserToken>
    {
        Task<UpUserToken> GetByUserIdAsync(string id, CancellationToken cancellationToken = default);
    }
}