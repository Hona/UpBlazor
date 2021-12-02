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
            await using var session = Store.LightweightSession();
            
            session.Insert(model);

            await session.SaveChangesAsync();
        }

        public async Task UpdateAsync(T model)
        {
            await using var session = Store.LightweightSession();
            
            session.Update(model);

            await session.SaveChangesAsync();
            
        }

        public async Task AddOrUpdateAsync(T model)
        {
            await using var session = Store.LightweightSession();
            
            session.Store(model);

            await session.SaveChangesAsync();
        }

        public async Task DeleteAsync(T model)
        {
            await using var session = Store.LightweightSession();
            
            session.Delete(model);

            await session.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            await using var session = Store.QuerySession();

            return await session.Query<T>().ToListAsync();
        }
    }
}