using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Marten;
using UpBlazor.Core.Models;
using UpBlazor.Infrastructure.Migrations.Core;

namespace UpBlazor.Infrastructure.Migrations;

public class Auth0_UserIds : IMigration
{
    public int Version => 01;

    public async Task MigrateAsync(IDocumentStore store)
    {
        await using var session = store.LightweightSession();

        await MigrateModelAsync<RegisteredUser>(session, 
            x => x.Id, 
            (x, newId) => x.Id = newId);
        
        await MigrateModelAsync<Expense>(session, 
            x => x.UserId, 
            (x, newId) => x.UserId = newId);
        
        await MigrateModelAsync<Goal>(session, 
            x => x.UserId, 
            (x, newId) => x.UserId = newId);
        
        await MigrateModelAsync<Income>(session, 
            x => x.UserId, 
            (x, newId) => x.UserId = newId);
        
        await MigrateModelAsync<NormalizedAggregate>(session, 
            x => x.UserId, 
            (x, newId) => x.UserId = newId);
        
        await MigrateModelAsync<NotificationRead>(session, 
            x => x.UserId, 
            (x, newId) => x.UserId = newId);
        
        await MigrateModelAsync<RecurringExpense>(session, 
            x => x.UserId, 
            (x, newId) => x.UserId = newId);
        
        await MigrateModelAsync<TwoUp>(session, 
            x => x.UserId1, 
            (x, newId) => x.UserId1 = newId);
        
        // 2 pulls of the same thing...
        await MigrateModelAsync<TwoUp>(session, 
            x => x.UserId2, 
            (x, newId) => x.UserId2 = newId);
        
        await MigrateModelAsync<TwoUpRequest>(session, 
            x => x.RequesterId, 
            (x, newId) => x.RequesterId = newId);
        
        // 2 pulls of the same thing...
        await MigrateModelAsync<TwoUpRequest>(session, 
            x => x.RequesteeId, 
            (x, newId) => x.RequesteeId = newId);
        
        await MigrateModelAsync<UpUserToken>(session, 
            x => x.UserId, 
            (x, newId) => x.UserId = newId);

        session.Store(new MigrationLog
        {
            Timestamp = DateTime.UtcNow,
            Version = Version
        });
        
        await session.SaveChangesAsync();
    }

    private static async Task MigrateModelAsync<T>(IDocumentSession session, Func<T, string> idField, Action<T, string> setIdField)
    {
        var models = await session
            .Query<T>()
            .ToListAsync();
        
        foreach (var model in models)
        {
            if (idField(model) is null || idField(model).StartsWith("windowslive|") || idField(model).Contains('|'))
            {
                continue;
            }

            var newId = $"windowslive|{idField(model)}";

            setIdField(model, newId);
        }
        
        session.Store((IEnumerable<T>)models);
    }
}