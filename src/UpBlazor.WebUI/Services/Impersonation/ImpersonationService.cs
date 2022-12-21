namespace UpBlazor.WebUI.Services.Impersonation;

public class ImpersonationService
{
    public string? ActorUserId { get; set; }

    public void Clear() => ActorUserId = null;
    
    public bool IsImpersonating => !string.IsNullOrEmpty(ActorUserId);
}