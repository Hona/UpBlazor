using System.Threading;
using System.Threading.Tasks;
using UpBlazor.Core.Models;

namespace UpBlazor.Core.Repositories
{
    public interface IUpUserTokenRepository : IGenericRepository<UpUserToken>
    {
        Task<UpUserToken> GetByUserIdAsync(string id, CancellationToken cancellationToken = default);
    }
}