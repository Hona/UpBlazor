using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

            return services;
        }
    }
}
