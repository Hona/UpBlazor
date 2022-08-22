using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Marten;
using Microsoft.Extensions.Hosting;
using UpBlazor.Infrastructure.Migrations.Core;

namespace UpBlazor.Infrastructure.Services;

public class MartenHostedService : IHostedService
{
    private readonly IDocumentStore _store;

    public MartenHostedService(IDocumentStore store)
    {
        _store = store;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _store.Storage.ApplyAllConfiguredChangesToDatabaseAsync();
        
        // Migrate all data
        await using var session = _store.QuerySession();

        var migrationHistory = await session.Query<MigrationLog>()
            .ToListAsync(token: cancellationToken);
        
        var latestMigration = migrationHistory
            .MaxBy(x => x.Version)?
            .Version ?? -1;

        var migrationType = typeof(IMigration);
        var assembly = migrationType.Assembly;
        
        var migrations = assembly.GetTypes()
            .Where(x => x.IsClass && !x.IsAbstract && migrationType.IsAssignableFrom(x))
            .Select(x => (IMigration) Activator.CreateInstance(x))
            .OrderBy(x => x?.Version)
            .Where(x => x?.Version > latestMigration)
            .ToList();

        foreach (var migration in migrations)
        {
            await migration.MigrateAsync(_store);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}