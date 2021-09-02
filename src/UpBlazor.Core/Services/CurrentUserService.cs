using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Up.NET.Api;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Core.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly IUpUserTokenRepository _upUserTokenRepository;
        public async Task<IEnumerable<Claim>> GetClaimsAsync()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();

            var user = authState.User;

            if (user?.Identity?.IsAuthenticated ?? false)
            {
                return user.Claims;
            }

            return null;
        }

        public async Task<string> GetGivenNameAsync()
        {
            var claims = await GetClaimsAsync();

            return claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value;
        }

        private UpApi _upApi;

        public CurrentUserService(AuthenticationStateProvider authenticationStateProvider, IUpUserTokenRepository upUserTokenRepository)
        {
            _authenticationStateProvider = authenticationStateProvider;
            _upUserTokenRepository = upUserTokenRepository;
        }

        public async Task<UpApi> GetApiAsync(bool forceReload = false)
        {
            if (!forceReload && _upApi != null)
            {
                return _upApi;
            }
            
            var userId = await GetUserIdAsync();

            var userToken = await _upUserTokenRepository.GetByUserIdAsync(userId);

            if (userToken == null)
            {
                return null;
            }

            _upApi = new UpApi(userToken.AccessToken);
            return _upApi;
        }

        public async Task<string> GetUserIdAsync()
        {
            var claims = await GetClaimsAsync();
            return claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ??
                   throw new InvalidOperationException("Logged in user must have a ID claim");
        }
    }
}