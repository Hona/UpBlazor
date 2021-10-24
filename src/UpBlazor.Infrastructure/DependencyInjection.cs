using Marten;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpBlazor.Core.Models;
using UpBlazor.Core.Repositories;
using UpBlazor.Infrastructure.Repositories;

namespace UpBlazor.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMarten(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMarten(options =>
            {
                options.Connection(configuration.GetConnectionString("Marten"));

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

            return services;
        }
    }
}
