using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Up.NET.Api;
using UpBlazor.Core.Models.Mock;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Application.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly IUpUserTokenRepository _upUserTokenRepository;
        private readonly IRegisteredUserRepository _registeredUserRepository;

        private string _impersonationUserId;

        private IUpApi _upApi;

        public CurrentUserService(AuthenticationStateProvider authenticationStateProvider, IUpUserTokenRepository upUserTokenRepository, IRegisteredUserRepository registeredUserRepository)
        {
            _authenticationStateProvider = authenticationStateProvider;
            _upUserTokenRepository = upUserTokenRepository;
            _registeredUserRepository = registeredUserRepository;
        }

        public async Task<IUpApi> GetApiAsync(string overrideToken = null, bool forceReload = false)
        {
            if (!forceReload && string.IsNullOrWhiteSpace(overrideToken) && _upApi != null)
            {
                return _upApi;
            }

            var userId = await GetUserIdAsync().ConfigureAwait(false);

            var userToken = await _upUserTokenRepository.GetByUserIdAsync(userId).ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(overrideToken) && userToken != null)
            {
                userToken.AccessToken = overrideToken;
            }

            var accessToken = userToken?.AccessToken ?? overrideToken;

            switch (accessToken)
            {
                case null:
                    return null;
                case MockUpApi.MockUpToken:
                    _upApi = new MockUpApi();
                    break;
                default:
                    _upApi = new UpApi(accessToken);
                    break;
            }

            return _upApi;
        }

        public async Task<string> GetUserIdAsync()
        {
            var claims = await GetClaimsAsync().ConfigureAwait(false);
            return claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ??
                   throw new InvalidOperationException("Logged in user must have a ID claim");
        }

        public async Task<IEnumerable<Claim>> GetClaimsAsync()
        {
            if (!string.IsNullOrWhiteSpace(_impersonationUserId))
            {
                var cachedUser = await _registeredUserRepository.GetByIdAsync(_impersonationUserId).ConfigureAwait(false);

                return new List<Claim>
                {
                    new(ClaimTypes.Email, cachedUser.Email),
                    new(ClaimTypes.NameIdentifier, cachedUser.Id),
                    new(ClaimTypes.GivenName, cachedUser.GivenName)
                };
            }

            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync().ConfigureAwait(false);

            var user = authState.User;

            if (user?.Identity?.IsAuthenticated ?? false)
            {
                return user.Claims;
            }

            return null;
        }

        public async Task<string> GetGivenNameAsync()
        {
            var claims = await GetClaimsAsync().ConfigureAwait(false);

            return claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value;
        }

        public void Impersonate(string userId)
        {
            _impersonationUserId = userId;
            _upApi = null;
        }

        public void ResetImpersonation()
        {
            _impersonationUserId = null;
            _upApi = null;
        }
    }
}