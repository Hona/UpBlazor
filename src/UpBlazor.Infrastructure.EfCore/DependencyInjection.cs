using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UpBlazor.Core.Repositories;
using UpBlazor.Infrastructure.EfCore.Repositories;

namespace UpBlazor.Infrastructure.EfCore
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddEfCore(this IServiceCollection services, IConfiguration configuration, DbType dbType = DbType.SqlServer)
        {
            services.AddDbContextPool<UpBankDbContext>(options =>
            {
                switch (dbType)
                {
                    case DbType.SqlServer:
                        options.UseSqlServer(configuration.GetConnectionString("SqlServer"));
                        break;
                    case DbType.Sqlite:
                        // Using SQLite by default as it is easier setup.
                        options.UseSqlite("Data Source=db.sqlite");
                        break;
                    case DbType.SqliteInMemory:
                        options.UseSqlite("DataSource=:memory:", x => { });
                        break;
                    default:
                        break;
                }

#if DEBUG
                // Most project shouldn't expose sensitive data, which is why we are
                // limiting to be available only in DEBUG mode.
                // If this is not, SQL "parameters" will be '?' instead of actual values.
                options.EnableSensitiveDataLogging();
#endif
            });

            services.AddTransient<IUpUserTokenRepository, UpUserTokenRepository>();
            services.AddTransient<ITwoUpRepository, TwoUpRepository>();
            services.AddTransient<ITwoUpRequestRepository, TwoUpRequestRepository>();
            services.AddTransient<IRegisteredUserRepository, RegisteredUserRepository>();
            services.AddTransient<IExpenseRepository, ExpenseRepository>();
            services.AddTransient<IRecurringExpenseRepository, RecurringExpenseRepository>();
            services.AddTransient<IIncomeRepository, IncomeRepository>();
            services.AddTransient<IGoalRepository, GoalRepository>();
            services.AddTransient<ISavingsPlanRepository, SavingsPlanRepository>();
            services.AddTransient<INormalizedAggregateRepository, NormalizedAggregateRepository>();

            return services;
        }
    }
}
