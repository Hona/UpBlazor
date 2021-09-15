using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Marten;
using Marten.Linq;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly IDocumentStore Store;
        protected readonly IDocumentSession Session;

        protected IMartenQueryable<T> Queryable => Session.Query<T>();

        protected GenericRepository(IDocumentStore store)
        {
            Store = store;
            Session = Store.LightweightSession();
        }

        public async Task AddAsync(T model)
        {
            Session.Insert(model);

            await Session.SaveChangesAsync();
        }

        public async Task UpdateAsync(T model)
        {
            Session.Update(model);

            await Session.SaveChangesAsync();
        }

        public async Task AddOrUpdateAsync(T model)
        {
            Session.Store(model);

            await Session.SaveChangesAsync();
        }

        public async Task DeleteAsync(T model)
        {
            Session.Delete(model);

            await Session.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync() => await Session.Query<T>().ToListAsync();
    }
}