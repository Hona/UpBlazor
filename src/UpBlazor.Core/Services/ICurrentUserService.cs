using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Up.NET.Api;

namespace UpBlazor.Core.Services
{
    public interface ICurrentUserService
    {
        Task<UpApi> GetApiAsync(bool forceReload = false);
        Task<string> GetUserIdAsync();
        Task<IEnumerable<Claim>> GetClaimsAsync();
        Task<string> GetGivenNameAsync();
    }
}