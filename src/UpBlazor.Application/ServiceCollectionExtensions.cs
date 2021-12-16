using MediatR;
using Microsoft.Extensions.DependencyInjection;
using UpBlazor.Application.Features.Forecast;
using UpBlazor.Application.Features.Notifications;
using UpBlazor.Application.Services;

namespace UpBlazor.Application;

public static class ServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddSingleton<INormalizerService, NormalizerService>();
        services.AddScoped<IForecastService, ForecastService>();
        
        services.AddMediatR(typeof(GetAllNotificationsQuery));

#if DEBUG
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
#endif
    }
}