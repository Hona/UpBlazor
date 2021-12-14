using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Marten;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly IDocumentStore Store;

        protected GenericRepository(IDocumentStore store)
        {
            Store = store;
        }

        public async Task AddAsync(T model, CancellationToken cancellationToken = default)
        {
            await using var session = Store.LightweightSession();
            
            session.Insert(model);

            await session.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(T model, CancellationToken cancellationToken = default)
        {
            await using var session = Store.LightweightSession();
            
            session.Update(model);

            await session.SaveChangesAsync(cancellationToken);
            
        }

        public async Task AddOrUpdateAsync(T model, CancellationToken cancellationToken = default)
        {
            await using var session = Store.LightweightSession();
            
            session.Store(model);

            await session.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(T model, CancellationToken cancellationToken = default)
        {
            await using var session = Store.LightweightSession();
            
            session.Delete(model);

            await session.SaveChangesAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            await using var session = Store.QuerySession();

            return await session.Query<T>()
                .ToListAsync(cancellationToken);
        }
    }
}