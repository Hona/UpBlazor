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

        public async Task<RegisteredUser> GetByIdAsync(string id)
        {
            using var session = Store.QuerySession();

            return await session.Query<RegisteredUser>()
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<RegisteredUser> GetByEmailAsync(string email)
        {
            using var session = Store.QuerySession();

            return await session.Query<RegisteredUser>()
                .SingleOrDefaultAsync(x => x.Email == email);
        }

        public async Task<IReadOnlyList<RegisteredUser>> GetAllByIds(params string[] ids)
        {
            using var session = Store.QuerySession();

            var output = new List<RegisteredUser>();

            // TODO: Revisit this query .Where(x => ids.Contains(x.Id)) errors.
            foreach (var id in ids)
            {
                var foundUser = await session.Query<RegisteredUser>()
                    .SingleOrDefaultAsync(x => x.Id == id);

                output.Add(foundUser);
            }

            return output.AsReadOnly();
        }
    }
}