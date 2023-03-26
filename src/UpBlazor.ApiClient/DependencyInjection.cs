using Microsoft.Extensions.DependencyInjection;

namespace UpBlazor.ApiClient;

public static class DependencyInjection
{
    public static void AddApiClients(this IServiceCollection services, string namedHttpClient)
    {
        services.AddHttpClient<ExpensesClient>(namedHttpClient);
        services.AddHttpClient<ForecastClient>(namedHttpClient);
        services.AddHttpClient<IncomesClient>(namedHttpClient);
        services.AddHttpClient<NormalizedClient>(namedHttpClient);
        services.AddHttpClient<NotificationsClient>(namedHttpClient);
        services.AddHttpClient<UpClient>(namedHttpClient);
        services.AddHttpClient<PlannerClient>(namedHttpClient);
        services.AddHttpClient<UsersClient>(namedHttpClient);
        services.AddHttpClient<RecurringExpensesClient>(namedHttpClient);
        services.AddHttpClient<SavingsPlanClient>(namedHttpClient);
    }
}