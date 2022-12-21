using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using UpBlazor.ApiClient;
using UpBlazor.WebUI;
using UpBlazor.WebUI.Services.Impersonation;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddAntDesign();

builder.Services.AddMudServices();

builder.Services.AddAuthorizationCore(options =>
{
    options.AddPolicy("Admin", 
        policy => policy.RequireAssertion(context =>  context.User
            .FindFirst("sub")?.Value is "windowslive|2a73b0d97086ad1d"));
});

builder.Services.AddAuth0OidcAuthentication(options =>
{
    builder.Configuration.Bind("Auth0", options.ProviderOptions);
    options.ProviderOptions.ResponseType = "code";
    
    // API audience
    options.ProviderOptions.AdditionalProviderParameters.Add("audience", builder.Configuration["Auth0:Audience"]);
    
    // Process logouts
    options.ProviderOptions.MetadataSeed.EndSessionEndpoint = $"{builder.Configuration["Auth0:Authority"]}/v2/logout?client_id={builder.Configuration["Auth0:ClientId"]}&returnTo={builder.HostEnvironment.BaseAddress}";
});

builder.Services.AddSingleton<ImpersonationService>();
builder.Services.AddScoped<ImpersonationMessageHandler>();

builder.Services.AddScoped<string>(x => builder.Configuration["ApiUri"] ?? throw new InvalidOperationException());

const string ApiClient = nameof(ApiClient);
builder.Services.AddHttpClient(ApiClient,
        client => client.BaseAddress = new Uri(builder.Configuration["ApiUri"]))
    .AddHttpMessageHandler(sp =>
        sp.GetRequiredService<AuthorizationMessageHandler>()
            .ConfigureHandler(
                authorizedUrls: new[] { builder.Configuration["ApiUri"] }
            )
    ).AddHttpMessageHandler<ImpersonationMessageHandler>();

builder.Services.AddHttpClient<ExpensesClient>(ApiClient);
builder.Services.AddHttpClient<ForecastClient>(ApiClient);
builder.Services.AddHttpClient<IncomesClient>(ApiClient);
builder.Services.AddHttpClient<NormalizedClient>(ApiClient);
builder.Services.AddHttpClient<NotificationsClient>(ApiClient);
builder.Services.AddHttpClient<UpClient>(ApiClient);
builder.Services.AddHttpClient<PlannerClient>(ApiClient);
builder.Services.AddHttpClient<UsersClient>(ApiClient);
builder.Services.AddHttpClient<RecurringExpensesClient>(ApiClient);
builder.Services.AddHttpClient<SavingsPlanClient>(ApiClient);

await builder.Build().RunAsync();