using Microsoft.EntityFrameworkCore;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Infrastructure.EfCore.Repositories
{
    internal class GenericRepository<T> : IGenericRepository<T>
        where T : class
    {
        public GenericRepository(UpBankDbContext context, DbSet<T> dbSet)
        {
            Context = context;
            DbSet = dbSet;
        }

        public UpBankDbContext Context { get; }
        public DbSet<T> DbSet { get; }

        public async Task AddAsync(T model)
        {
            await DbSet.AddAsync(model);
            await Context.SaveChangesAsync();
        }

        public async Task AddOrUpdateAsync(T model)
        {
            DbSet.Attach(model);
            await Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T model)
        {
            DbSet.Remove(model);
            await Context.SaveChangesAsync();
        }

        public IQueryable<T> Query() => DbSet.AsQueryable();

        public async Task UpdateAsync(T model)
        {
            DbSet.Update(model);
            await Context.SaveChangesAsync();
        }
    }
}
