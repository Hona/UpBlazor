using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;

namespace UpBlazor.WebUI;

public class ApiAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorageService;

    public ApiAuthenticationStateProvider(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }
    
    private AuthenticationState Anonymous => new(new ClaimsPrincipal(new ClaimsIdentity()));
    private AuthenticationState Authenticated(string userName, string[] claims) => new(new ClaimsPrincipal(new ClaimsIdentity(new[]
    {
        new Claim(ClaimTypes.Name, userName),
    }.Concat(claims.Select(x => new Claim(x, string.Empty))), "api")));

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var possiblyLoggedIn = await _localStorageService.ContainKeyAsync(LocalStorageKeys.LoggedIn);

        if (!possiblyLoggedIn)
        {
            return Anonymous;
        }
        
        var loggedInUntil = await _localStorageService.GetItemAsync<DateTime>(LocalStorageKeys.LoggedIn);

        if (loggedInUntil <= DateTime.Now)
        {
            return Anonymous;
        }
        
        var userName = await _localStorageService.GetItemAsync<string>(LocalStorageKeys.UserName);
        var claims = await _localStorageService.GetItemAsync<string[]>(LocalStorageKeys.Claims);
        
        return Authenticated(userName, claims);
    }
}