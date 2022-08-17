using System.Security.Claims;
using System.Text.Json.Serialization;
using Marten;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.HttpOverrides;
using UpBlazor.Application;
using UpBlazor.Core.Models;
using UpBlazor.Core.Repositories;
using UpBlazor.Infrastructure.Repositories;
using UpBlazor.Infrastructure.Services;
using UpBlazor.WebApi;
using Weasel.Core;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

builder.Services.AddControllers(options =>
{
    options.Filters.Add<HttpResponseExceptionFilter>();
})
    .AddJsonOptions(options =>
    {
        // Serialise enums as string
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
});

builder.Services.AddOpenApiDocument();

services.AddApplication();

services.AddAuthentication(MicrosoftAccountDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.AccessDeniedPath = "/access-denied";
    })
    .AddMicrosoftAccount(options =>
    {
        options.ClientId = builder.Configuration["Authentication:Microsoft:ClientId"];
        options.ClientSecret = builder.Configuration["Authentication:Microsoft:ClientSecret"];
        options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.AuthorizationEndpoint = MicrosoftAccountDefaults.AuthorizationEndpoint + "?prompt=select_account";

        if (builder.Environment.IsProduction())
        {
            // In prod we have a reverse proxy
            // authentication probably doesn't believe our forwarded headers
            options.CallbackPath = builder.Configuration["UiUri"] + "/api/signin-microsoft";
        }
        else
        {
            options.CallbackPath = "/api/signin-microsoft";
        }
    });

services.AddAuthorization(options =>
{
    options.AddPolicy(Constants.AllowedEmailAuthorizationPolicy, policy =>
    {
        policy.RequireAuthenticatedUser()
            .RequireClaim(ClaimTypes.Email)
            .RequireClaim(ClaimTypes.GivenName)
            .RequireAssertion(context =>
            {
                var emailAddress = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
                var allowedEmails = builder.Configuration
                    .GetSection(Constants.AllowedEmailAuthorizationPolicy)
                    .GetChildren()
                    .Select(x => x.Value)
                    .ToArray();

                if (!allowedEmails.Any())
                {
                    return true;
                }

                return allowedEmails.Any(x => x == emailAddress);
            });
    });

    options.AddPolicy(Constants.AdminAuthorizationPolicy, policy =>
    {
        policy.RequireAuthenticatedUser()
            .RequireClaim(ClaimTypes.Email)
            .RequireAssertion(context =>
            {
                var emailAddress = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)
                    ?.Value;

                var adminEmails = builder.Configuration
                    .GetSection(Constants.AdminAuthorizationPolicy)
                    .GetChildren()
                    .Select(x => x.Value)
                    .ToArray();

                if (!adminEmails.Any())
                {
                    return false;
                }

                return adminEmails.Any(x => x == emailAddress);
            });
    });
});

services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("Marten"));

    options.AutoCreateSchemaObjects = builder.Environment.IsDevelopment()
        ? AutoCreate.All
        : AutoCreate.CreateOrUpdate;

    options.Schema.For<UpUserToken>()
        .Identity(x => x.UserId);
    options.Schema.For<TwoUp>()
        .Identity(x => x.MartenId);
    options.Schema.For<TwoUpRequest>()
        .Identity(x => x.MartenId);
    options.Schema.For<NormalizedAggregate>()
        .Identity(x => x.UserId);
});

services.AddSingleton<IUpUserTokenRepository, UpUserTokenRepository>();
services.AddSingleton<ITwoUpRepository, TwoUpRepository>();
services.AddSingleton<ITwoUpRequestRepository, TwoUpRequestRepository>();
services.AddSingleton<IRegisteredUserRepository, RegisteredUserRepository>();
services.AddSingleton<IExpenseRepository, ExpenseRepository>();
services.AddSingleton<IRecurringExpenseRepository, RecurringExpenseRepository>();
services.AddSingleton<IIncomeRepository, IncomeRepository>();
services.AddSingleton<IGoalRepository, GoalRepository>();
services.AddSingleton<ISavingsPlanRepository, SavingsPlanRepository>();
services.AddSingleton<INormalizedAggregateRepository, NormalizedAggregateRepository>();
services.AddSingleton<INotificationRepository, NotificationRepository>();
services.AddSingleton<INotificationReadRepository, NotificationReadRepository>();

services.AddHostedService<MartenHostedService>();

services.AddAuthentication();
services.AddAuthorization();

services.AddHttpContextAccessor();

var app = builder.Build();

var forwardingOptions = new ForwardedHeadersOptions()
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost
};
forwardingOptions.KnownNetworks.Clear(); // Loopback by default, this should be temporary
forwardingOptions.KnownProxies.Clear(); // Update to include

app.UseForwardedHeaders(forwardingOptions);

if (app.Environment.IsProduction())
{
    app.Use((context, next) =>
    {
        context.Request.Scheme = "https";
        return next(context);
    });
}

app.UseCors(options =>
{
    options.AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
        .WithOrigins(builder.Configuration["UiUri"]);
});

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi3();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();