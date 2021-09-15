using Marten;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UpBlazor.Core.Models;
using UpBlazor.Core.Repositories;
using UpBlazor.Infrastructure.Repositories;

namespace UpBlazor.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static void AddUpBlazorMarten(this IServiceCollection services, string connectionString)
        {
            services.AddMarten(options =>
            {
                options.Connection(connectionString);

                options.AutoCreateSchemaObjects = AutoCreate.All;

                options.Schema.For<UpUserToken>()
                    .Identity(x => x.UserId);
                options.Schema.For<TwoUp>()
                    .Identity(x => x.MartenId);
                options.Schema.For<TwoUpRequest>()
                    .Identity(x => x.MartenId);
                options.Schema.For<NormalizedAggregate>()
                    .Identity(x => x.UserId);
            });
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUpUserTokenRepository, UpUserTokenRepository>();
            services.AddScoped<ITwoUpRepository, TwoUpRepository>();
            services.AddScoped<ITwoUpRequestRepository, TwoUpRequestRepository>();
            services.AddScoped<IRegisteredUserRepository, RegisteredUserRepository>();
            services.AddScoped<IExpenseRepository, ExpenseRepository>();
            services.AddScoped<IRecurringExpenseRepository, RecurringExpenseRepository>();
            services.AddScoped<IIncomeRepository, IncomeRepository>();
            services.AddScoped<IGoalRepository, GoalRepository>();
            services.AddScoped<ISavingsPlanRepository, SavingsPlanRepository>();
            services.AddScoped<INormalizedAggregateRepository, NormalizedAggregateRepository>();
        }
    }
}