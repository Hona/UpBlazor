using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Marten;
using UpBlazor.Application.Repositories;
using UpBlazor.Domain.Entities;

namespace UpBlazor.Infrastructure.Repositories
{
    public class RegisteredUserRepository : GenericRepository<RegisteredUser>, IRegisteredUserRepository
    {
        public RegisteredUserRepository(IDocumentStore store) : base(store) { }

        public new async Task AddOrUpdateAsync(RegisteredUser model, CancellationToken cancellationToken = default)
        {
            await using var session = Store.LightweightSession();

            var existingModel = await session.Query<RegisteredUser>()
                .SingleOrDefaultAsync(x => x.Id == model.Id, cancellationToken);

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

            await session.SaveChangesAsync(cancellationToken);
        }

        public async Task<RegisteredUser> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            await using var session = Store.QuerySession();

            return await session.Query<RegisteredUser>()
                .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<RegisteredUser> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            await using var session = Store.QuerySession();

            return await session.Query<RegisteredUser>()
                .SingleOrDefaultAsync(x => x.Email == email, cancellationToken);
        }

        public async Task<IReadOnlyList<RegisteredUser>> GetAllByIdsAsync(CancellationToken cancellationToken = default, params string[] ids)
        {
            await using var session = Store.QuerySession();

            return await session.Query<RegisteredUser>()
                .Where(x => x.Id.IsOneOf(ids))
                .ToListAsync(cancellationToken);
        }
    }
}