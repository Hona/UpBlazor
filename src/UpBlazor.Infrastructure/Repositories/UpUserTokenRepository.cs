using System.Threading.Tasks;
using Marten;
using UpBlazor.Core.Models;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Infrastructure.Repositories
{
    public class UpUserTokenRepository : GenericRepository<UpUserToken>, IUpUserTokenRepository
    {
        public UpUserTokenRepository(IDocumentStore store) : base(store) { }

        public async Task<UpUserToken> GetByUserIdAsync(string id) => 
            await Queryable
                .SingleOrDefaultAsync(x => x.UserId == id);
    }
}