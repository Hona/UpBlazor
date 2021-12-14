using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Up.NET.Api;

namespace UpBlazor.Application.Services
{
    public interface ICurrentUserService
    {
        Task<IUpApi> GetApiAsync(string overrideToken = null, bool forceReload = false, CancellationToken cancellationToken = default);
        Task<string> GetUserIdAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<Claim>> GetClaimsAsync(CancellationToken cancellationToken = default);
        Task<string> GetGivenNameAsync(CancellationToken cancellationToken = default);
        public bool IsImpersonating { get; }
        void Impersonate(string userId);
        void ResetImpersonation();
    }
}