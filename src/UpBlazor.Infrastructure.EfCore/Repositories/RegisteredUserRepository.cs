using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UpBlazor.Core.Models;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Infrastructure.EfCore.Repositories
{
    internal class RegisteredUserRepository : GenericRepository<RegisteredUser>, IRegisteredUserRepository
    {
        public RegisteredUserRepository(UpBankDbContext context) : base(context, context.RegisteredUsers) { }

        public new async Task AddOrUpdateAsync(RegisteredUser model)
        {
            var existingModel = await DbSet
                .SingleOrDefaultAsync(x => x.Id == model.Id);

            if (existingModel == null)
            {
                model.CreatedAt = DateTime.Now;
                model.UpdatedAt = null;

                await DbSet.AddAsync(model);
            }
            else
            {
                existingModel.UpdatedAt = DateTime.Now;
            }

            await Context.SaveChangesAsync();
        }

        public async Task<RegisteredUser> GetByIdAsync(string id)
            => await DbSet
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);

        public async Task<RegisteredUser> GetByEmailAsync(string email)
            => await DbSet
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Email == email);

        public async Task<IReadOnlyList<RegisteredUser>> GetAllByIdsAsync(params string[] ids)
            => await DbSet
                .AsNoTracking()
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();

        public async Task<IReadOnlyList<RegisteredUser>> GetAllAsync()
            => await DbSet
                .AsNoTracking()
                .ToListAsync();
    }
}
