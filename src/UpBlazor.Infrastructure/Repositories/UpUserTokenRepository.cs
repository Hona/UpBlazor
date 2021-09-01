using System.Threading.Tasks;
using Marten;
using UpBlazor.Core.Models;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Infrastructure.Repositories
{
    public class UpUserTokenRepository : GenericRepository<UpUserToken>, IUpUserTokenRepository
    {
        public UpUserTokenRepository(IDocumentStore store) : base(store) { }

        public Task<UpUserToken> GetByUserIdAsync(string id)
        {
            using var session = Store.QuerySession();

            return session.Query<UpUserToken>()
                .SingleOrDefaultAsync(x => x.UserId == id);
        }
    }
}