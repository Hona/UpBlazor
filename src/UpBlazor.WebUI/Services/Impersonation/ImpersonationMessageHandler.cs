namespace UpBlazor.WebUI.Services.Impersonation;

public class ImpersonationMessageHandler : DelegatingHandler
{
    private readonly ImpersonationService _impersonationService;

    public ImpersonationMessageHandler(ImpersonationService impersonationService)
    {
        _impersonationService = impersonationService;
    }
    
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (_impersonationService.IsImpersonating)
        {
            request.Headers.Add("x-upblazor-impersonate", _impersonationService.ActorUserId);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}