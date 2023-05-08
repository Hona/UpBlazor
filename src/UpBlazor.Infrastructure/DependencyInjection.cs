using System;
using Marten;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UpBlazor.Application.Repositories;
using UpBlazor.Domain.Entities;
using UpBlazor.Domain.Entities.Normalized;
using UpBlazor.Infrastructure.Migrations.Core;
using UpBlazor.Infrastructure.Repositories;
using UpBlazor.Infrastructure.Services;
using Weasel.Core;

namespace UpBlazor.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration config, IWebHostEnvironment environment)
    {
        services.AddMarten(options =>
        {
            options.Connection(config.GetConnectionString("Marten") ?? throw new InvalidOperationException());

            options.AutoCreateSchemaObjects = environment.IsDevelopment()
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
    }
}