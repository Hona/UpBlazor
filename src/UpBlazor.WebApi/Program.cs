using System.Security.Claims;
using System.Text.Json.Serialization;
using Marten;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.HttpOverrides;
using UpBlazor.Application;
using UpBlazor.Application.Services;
using UpBlazor.Core.Models;
using UpBlazor.Core.Repositories;
using UpBlazor.Infrastructure.Migrations.Core;
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

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, c =>
    {
        c.Authority = builder.Configuration["Auth0:Authority"];
        
        c.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidAudience = builder.Configuration["Auth0:Audience"],
            ValidIssuer = builder.Configuration["Auth0:Domain"]
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

services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("Marten") ?? throw new InvalidOperationException());

    options.AutoCreateSchemaObjects = builder.Environment.IsDevelopment()
        ? AutoCreate.All
        : AutoCreate.CreateOrUpdate;

    options.Schema.For<UpUserToken>()
        .Identity(x => x.UserId);
    options.Schema.For<NormalizedAggregate>()
        .Identity(x => x.UserId);
    options.Schema.For<MigrationLog>()
        .Identity(x => x.Version);
});

services.AddSingleton<IUpUserTokenRepository, UpUserTokenRepository>();
services.AddSingleton<IRegisteredUserRepository, RegisteredUserRepository>();
services.AddSingleton<IExpenseRepository, ExpenseRepository>();
services.AddSingleton<IRecurringExpenseRepository, RecurringExpenseRepository>();
services.AddSingleton<IIncomeRepository, IncomeRepository>();
services.AddSingleton<ISavingsPlanRepository, SavingsPlanRepository>();
services.AddSingleton<INormalizedAggregateRepository, NormalizedAggregateRepository>();
services.AddSingleton<INotificationRepository, NotificationRepository>();
services.AddSingleton<INotificationReadRepository, NotificationReadRepository>();

services.AddHostedService<MartenHostedService>();

services.AddAuthentication();
services.AddAuthorization();

services.AddHttpContextAccessor();

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

services.AddHttpLogging(options =>
{
    options.LoggingFields = HttpLoggingFields.RequestPropertiesAndHeaders;
});

var app = builder.Build();

app.UseForwardedHeaders();

app.UseCors(options =>
{
    options.AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
        .WithOrigins(builder.Configuration["UiUri"] ?? throw new InvalidOperationException());
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