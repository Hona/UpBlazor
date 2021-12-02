using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Marten;
using UpBlazor.Core.Models;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Infrastructure.Repositories
{
    public class RegisteredUserRepository : GenericRepository<RegisteredUser>, IRegisteredUserRepository
    {
        public RegisteredUserRepository(IDocumentStore store) : base(store) { }

        public new async Task AddOrUpdateAsync(RegisteredUser model)
        {
            await using var session = Store.LightweightSession();

            var existingModel = await session.Query<RegisteredUser>()
                .SingleOrDefaultAsync(x => x.Id == model.Id);

            if (existingModel == null)
            {
                model.CreatedAt = DateTime.Now;
                model.UpdatedAt = null;
            }
            else
            {
                model.UpdatedAt = DateTime.Now;
            }

            session.Store(model);

            await session.SaveChangesAsync();
        }

        public async Task<RegisteredUser> GetByIdAsync(string id)
        {
            await using var session = Store.QuerySession();

            return await session.Query<RegisteredUser>()
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<RegisteredUser> GetByEmailAsync(string email)
        {
            await using var session = Store.QuerySession();

            return await session.Query<RegisteredUser>()
                .SingleOrDefaultAsync(x => x.Email == email);
        }

        public async Task<IReadOnlyList<RegisteredUser>> GetAllByIdsAsync(params string[] ids)
        {
            await using var session = Store.QuerySession();

            return await session.Query<RegisteredUser>()
                .Where(x => x.Id.IsOneOf(ids))
                .ToListAsync();
        }
    }
}