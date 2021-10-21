using System.Linq;
using System.Threading.Tasks;
using Marten;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly IDocumentStore Store;
        protected readonly IDocumentSession Session;

        protected GenericRepository(IDocumentStore store)
        {
            Store = store;
            Session = Store.LightweightSession();
        }

        public async Task AddAsync(T model)
        {
            using var session = Store.LightweightSession();
            
            session.Insert(model);

            await session.SaveChangesAsync();
        }

        public async Task UpdateAsync(T model)
        {
            using var session = Store.LightweightSession();
            
            session.Update(model);

            await session.SaveChangesAsync();
            
        }

        public async Task AddOrUpdateAsync(T model)
        {
            using var session = Store.LightweightSession();
            
            session.Store(model);

            await session.SaveChangesAsync();
        }

        public async Task DeleteAsync(T model)
        {
            using var session = Store.LightweightSession();
            
            session.Delete(model);

            await session.SaveChangesAsync();
        }

        public IQueryable<T> Query() => Session.Query<T>();
    }
}