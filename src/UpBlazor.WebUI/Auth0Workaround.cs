using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace UpBlazor.WebUI;

public static class AuthExtensions
{
    /// <summary>
    /// Adds support for Auth0 authentication for SPA applications using <see cref="Auth0OidcProviderOptions"/> and the <see cref="RemoteAuthenticationState"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <param name="configure">An action that will configure the <see cref="RemoteAuthenticationOptions{TProviderOptions}"/>.</param>
    /// <returns>The <see cref="IServiceCollection"/> where the services were registered.</returns>
    public static IRemoteAuthenticationBuilder<RemoteAuthenticationState, RemoteUserAccount> AddAuth0OidcAuthentication(this IServiceCollection services, Action<RemoteAuthenticationOptions<Auth0OidcProviderOptions>> configure)
    {
        services.TryAddEnumerable(ServiceDescriptor.Scoped<IPostConfigureOptions<RemoteAuthenticationOptions<Auth0OidcProviderOptions>>, DefaultAuth0OidcOptionsConfiguration>());
        return services.AddRemoteAuthentication<RemoteAuthenticationState, RemoteUserAccount, Auth0OidcProviderOptions>(configure);
    }
}

public class Auth0OidcProviderOptions : OidcProviderOptions
{
    public MetadataSeed MetadataSeed { get; set; } = new ();
}

public class MetadataSeed
{
    [JsonPropertyName("end_session_endpoint")]
    public string EndSessionEndpoint { get; set; } = null!;
}

// Copy/paste from Microsoft.AspNetCore.Components.WebAssembly.Authentication with the option type changed.
internal class DefaultAuth0OidcOptionsConfiguration : IPostConfigureOptions<RemoteAuthenticationOptions<Auth0OidcProviderOptions>>
{
    private readonly NavigationManager _navigationManager;

    public DefaultAuth0OidcOptionsConfiguration(NavigationManager navigationManager) => _navigationManager = navigationManager;

    public void Configure(RemoteAuthenticationOptions<Auth0OidcProviderOptions> options)
    {
        options.UserOptions.AuthenticationType ??= options.ProviderOptions.ClientId;

        var redirectUri = options.ProviderOptions.RedirectUri;
        if (redirectUri == null || !Uri.TryCreate(redirectUri, UriKind.Absolute, out _))
        {
            redirectUri ??= "authentication/login-callback";
            options.ProviderOptions.RedirectUri = _navigationManager
                .ToAbsoluteUri(redirectUri).AbsoluteUri;
        }

        var logoutUri = options.ProviderOptions.PostLogoutRedirectUri;
        if (logoutUri == null || !Uri.TryCreate(logoutUri, UriKind.Absolute, out _))
        {
            logoutUri ??= "authentication/logout-callback";
            options.ProviderOptions.PostLogoutRedirectUri = _navigationManager
                .ToAbsoluteUri(logoutUri).AbsoluteUri;
        }
    }

    public void PostConfigure(string name, RemoteAuthenticationOptions<Auth0OidcProviderOptions> options)
    {
        if (string.Equals(name, Options.DefaultName))
        {
            Configure(options);
        }
    }
}