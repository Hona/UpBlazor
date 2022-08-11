using System.Net;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace UpBlazor.WebUI;

public class ApiCookieInjector : DelegatingHandler
{
    private readonly ImpersonationService _impersonationService;
    private readonly ILocalStorageService _localStorageService;
    public ApiCookieInjector(ImpersonationService impersonationService, ILocalStorageService localStorageService)
    {
        _impersonationService = impersonationService;
        _localStorageService = localStorageService;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        Console.WriteLine(_impersonationService.IsImpersonating);
        if (_impersonationService.IsImpersonating)
        {
            request.Headers.Add("x-upblazor-impersonate", _impersonationService.ActorUserId);
        }
        
        var response = await base.SendAsync(request, cancellationToken);

        if (response.Headers.Location != null && response.StatusCode == HttpStatusCode.Found && response.Headers.Location.AbsoluteUri.Contains("login"))
        {
            await _localStorageService.ClearAsync();
            response.StatusCode = HttpStatusCode.Unauthorized;
        }

        return response;
    }
}