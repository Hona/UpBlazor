using System.Collections.Generic;
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

        public async Task AddAsync(T model)
        {
            var session = Store.LightweightSession();
            await using var _ = session.ConfigureAwait(false);

            session.Insert(model);

            await session.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task UpdateAsync(T model)
        {
            var session = Store.LightweightSession();
            await using var _ = session.ConfigureAwait(false);

            session.Update(model);

            await session.SaveChangesAsync().ConfigureAwait(false);
            
        }

        public async Task AddOrUpdateAsync(T model)
        {
            var session = Store.LightweightSession();
            await using var _ = session.ConfigureAwait(false);

            session.Store(model);

            await session.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteAsync(T model)
        {
            var session = Store.LightweightSession();
            await using var _ = session.ConfigureAwait(false);

            session.Delete(model);

            await session.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            var session = Store.QuerySession();
            await using var _ = session.ConfigureAwait(false);

            return await session.Query<T>().ToListAsync().ConfigureAwait(false);
        }
    }
}