using System.Collections.Generic;
using System.Threading.Tasks;
using UpBlazor.Core.Models;

namespace UpBlazor.Core.Repositories
{
    public interface IRegisteredUserRepository : IGenericRepository<RegisteredUser>
    {
        Task<RegisteredUser> GetByIdAsync(string id);
        Task<RegisteredUser> GetByEmailAsync(string email);
        Task<IReadOnlyList<RegisteredUser>> GetAllByIds(params string[] ids);
    }
}