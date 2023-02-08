using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Up.NET.Api;
using UpBlazor.Application.Exceptions;
using UpBlazor.Core.Models.Mock;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Application.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly HttpContext _httpContext;
    private readonly IUpUserTokenRepository _upUserTokenRepository;
    private readonly IRegisteredUserRepository _registeredUserRepository;

    private const string ImpersonationHeader = "x-upblazor-impersonate";
    
    private IUpApi _upApi;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor, IUpUserTokenRepository upUserTokenRepository, IRegisteredUserRepository registeredUserRepository)
    {
        _httpContext = httpContextAccessor.HttpContext;
        _upUserTokenRepository = upUserTokenRepository;
        _registeredUserRepository = registeredUserRepository;
    }

    public async Task<IUpApi> GetApiAsync(string overrideToken = null, bool forceReload = false, CancellationToken cancellationToken = default)
    {
        if (!forceReload && string.IsNullOrWhiteSpace(overrideToken) && _upApi != null)
        {
            return _upApi;
        }

        var userId = await GetUserIdAsync(cancellationToken);

        var userToken = await _upUserTokenRepository.GetByUserIdAsync(userId, cancellationToken);

        if (!string.IsNullOrWhiteSpace(overrideToken) && userToken != null)
        {
            userToken.AccessToken = overrideToken;
        }

        var accessToken = userToken?.AccessToken ?? overrideToken;

        _upApi = accessToken switch
        {
            null => throw new UpApiAccessTokenNotSetException(),
            MockUpApi.MockUpToken => new MockUpApi(),
            _ => new UpApi(accessToken)
        };

        return _upApi;
    }

    public async Task<string> GetUserIdAsync(CancellationToken cancellationToken = default)
    {
        var claims = await GetClaimsAsync(cancellationToken);
        return claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ??
               throw new InvalidOperationException("Logged in user must have a ID claim");
    }

    public async Task<IEnumerable<Claim>> GetClaimsAsync(CancellationToken cancellationToken = default)
    {
        if (IsImpersonating())
        {
            var cachedUser = await _registeredUserRepository.GetByIdAsync(ImpersonationUserId, cancellationToken);

            var claims = new List<Claim>()
            {
                new(ClaimTypes.NameIdentifier, cachedUser.Id)
            };

            if (cachedUser.Email is not null)
            {
                claims.Add(new(ClaimTypes.Email, cachedUser.Email));
            }

            if (cachedUser.GivenName is not null)
            {
                claims.Add(new(ClaimTypes.GivenName, cachedUser.GivenName));
            }
            
            return claims;
        }

        var user = _httpContext.User;

        if (user?.Identity?.IsAuthenticated ?? false)
        {
            return user.Claims;
        }

        return null;
    }

    public async Task<string> GetGivenNameAsync(CancellationToken cancellationToken = default)
    {
        var claims = await GetClaimsAsync(cancellationToken);

        return claims?.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value;
    }

    public bool IsAdmin() =>
        IsAdmin(_httpContext.User);
    
    public static bool IsAdmin(ClaimsPrincipal user) =>
        user
            .FindFirst(ClaimTypes.NameIdentifier)?.Value is "windowslive|2a73b0d97086ad1d";

    public bool IsImpersonating() => IsAdmin() && _httpContext.Request.Headers.ContainsKey(ImpersonationHeader);
    public string ImpersonationUserId => _httpContext.Request.Headers[ImpersonationHeader];
    
}