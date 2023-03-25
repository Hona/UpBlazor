using System.Threading;
using System.Threading.Tasks;

namespace UpBlazor.Application.Common.Services
{
    public interface INormalizerService
    {
        Task UpdateUserAsync(string userId, CancellationToken cancellationToken = default);
    }
}