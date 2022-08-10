using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using UpBlazor.ApiClient;
using UpBlazor.WebUI;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddAntDesign();

builder.Services.AddAuthorizationCore(options =>
{
    options.AddPolicy("Admins", policy => policy.RequireClaim("admin"));
    options.AddPolicy("AllowedEmails", policy => policy.RequireClaim("allowedEmails"));
});
builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped<string>(x => builder.Configuration["ApiUri"] ?? throw new InvalidOperationException());
builder.Services.AddHttpClient<ExpensesClient>().AddHttpMessageHandler<ApiCookieInjector>();
builder.Services.AddHttpClient<ForecastClient>().AddHttpMessageHandler<ApiCookieInjector>();
builder.Services.AddHttpClient<GoalsClient>().AddHttpMessageHandler<ApiCookieInjector>();
builder.Services.AddHttpClient<IncomesClient>().AddHttpMessageHandler<ApiCookieInjector>();
builder.Services.AddHttpClient<NormalizedClient>().AddHttpMessageHandler<ApiCookieInjector>();
builder.Services.AddHttpClient<NotificationsClient>().AddHttpMessageHandler<ApiCookieInjector>();
builder.Services.AddHttpClient<UpClient>().AddHttpMessageHandler<ApiCookieInjector>();
builder.Services.AddHttpClient<TwoUpClient>().AddHttpMessageHandler<ApiCookieInjector>();
builder.Services.AddHttpClient<PlannerClient>().AddHttpMessageHandler<ApiCookieInjector>();
builder.Services.AddHttpClient<UsersClient>().AddHttpMessageHandler<ApiCookieInjector>();
builder.Services.AddHttpClient<RecurringExpensesClient>().AddHttpMessageHandler<ApiCookieInjector>();
builder.Services.AddHttpClient<SavingsPlanClient>().AddHttpMessageHandler<ApiCookieInjector>();
builder.Services.AddScoped<ApiCookieInjector>();

await builder.Build().RunAsync();