using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.HttpOverrides;
using UpBlazor.Application.Common.Services;

namespace UpBlazor.WebApi;

public static class DependencyInjection
{
    public static void AddWebApi(this IServiceCollection services, IConfiguration config)
    {
        services.AddControllers(options =>
            {
                options.Filters.Add<HttpResponseExceptionFilter>();
            })
            .AddJsonOptions(options =>
            {
                // Serialise enums as string
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        services.AddRouting(options =>
        {
            options.LowercaseUrls = true;
        });

        services.AddOpenApiDocument();
     
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, c =>
            {
                c.Authority = config["Auth0:Authority"];
        
                c.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidAudience = config["Auth0:Audience"],
                    ValidIssuer = config["Auth0:Domain"]
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy(Constants.AdminAuthorizationPolicy, policy =>
            {
                policy
                    .RequireAuthenticatedUser()
                    .RequireAssertion(context => CurrentUserService.IsAdmin(context.User));
            });
        });
        
        services.AddAuthentication();
        services.AddAuthorization();
        
        services.AddHttpContextAccessor();
        
        // TODO: This probably isn't required now we host in Azure
        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                                       ForwardedHeaders.XForwardedProto;

            options.ForwardLimit = null;
    
            // Only loopback proxies are allowed by default.
            // Clear that restriction because forwarders are enabled by explicit 
            // configuration.
            options.KnownNetworks.Clear();
            options.KnownProxies.Clear();
        });
        
        // TODO: This is excessive for production, either only enable in dev or delete
        services.AddHttpLogging(options =>
        {
            options.LoggingFields = HttpLoggingFields.RequestPropertiesAndHeaders;
        });
    }
}