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
            var session = Store.LightweightSession();
            await using var _ = session.ConfigureAwait(false);

            var existingModel = await session.Query<RegisteredUser>()
                .SingleOrDefaultAsync(x => x.Id == model.Id).ConfigureAwait(false);

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

            await session.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<RegisteredUser> GetByIdAsync(string id)
        {
            var session = Store.QuerySession();
            await using var _ = session.ConfigureAwait(false);

            return await session.Query<RegisteredUser>()
                .SingleOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
        }

        public async Task<RegisteredUser> GetByEmailAsync(string email)
        {
            var session = Store.QuerySession();
            await using var _ = session.ConfigureAwait(false);

            return await session.Query<RegisteredUser>()
                .SingleOrDefaultAsync(x => x.Email == email).ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<RegisteredUser>> GetAllByIdsAsync(params string[] ids)
        {
            var session = Store.QuerySession();
            await using var _ = session.ConfigureAwait(false);

            return await session.Query<RegisteredUser>()
                .Where(x => x.Id.IsOneOf(ids))
                .ToListAsync().ConfigureAwait(false);
        }
    }
}