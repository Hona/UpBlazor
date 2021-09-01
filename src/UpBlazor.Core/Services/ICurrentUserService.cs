using System.Threading.Tasks;
using Up.NET.Api;

namespace UpBlazor.Core.Services
{
    public interface ICurrentUserService
    {
        Task<UpApi> GetApiAsync(bool forceReload = false);
        string GetUserId();
    }
}