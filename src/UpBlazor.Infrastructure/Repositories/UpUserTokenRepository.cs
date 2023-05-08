using System.Threading;
using System.Threading.Tasks;
using Marten;
using UpBlazor.Application.Repositories;
using UpBlazor.Domain.Entities;


namespace UpBlazor.Infrastructure.Repositories
{
    public class UpUserTokenRepository : GenericRepository<UpUserToken>, IUpUserTokenRepository
    {
        public UpUserTokenRepository(IDocumentStore store) : base(store) { }

        public async Task<UpUserToken> GetByUserIdAsync(string id, CancellationToken cancellationToken = default)
        {
            await using var session = Store.QuerySession();

            return await session.Query<UpUserToken>()
                .SingleOrDefaultAsync(x => x.UserId == id, cancellationToken);
        }
    }
}