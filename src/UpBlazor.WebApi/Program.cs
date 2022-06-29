using System.Security.Claims;
using Marten;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using UpBlazor.Application;
using UpBlazor.Core.Models;
using UpBlazor.Core.Repositories;
using UpBlazor.Infrastructure.Repositories;
using UpBlazor.Infrastructure.Services;
using UpBlazor.WebApi;
using Weasel.Postgresql;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

builder.Services.AddControllers(options =>
{
    options.Filters.Add<HttpResponseExceptionFilter>();
});

builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
});

builder.Services.AddOpenApiDocument();

services.AddApplication();

services.AddAuthentication(MicrosoftAccountDefaults.AuthenticationScheme)
    .AddCookie(options => { options.AccessDeniedPath = "/access-denied"; })
    .AddMicrosoftAccount(options =>
    {
        options.ClientId = builder.Configuration["Authentication:Microsoft:ClientId"];
        options.ClientSecret = builder.Configuration["Authentication:Microsoft:ClientSecret"];
        options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.AuthorizationEndpoint = MicrosoftAccountDefaults.AuthorizationEndpoint + "?prompt=select_account";
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

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi3();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();