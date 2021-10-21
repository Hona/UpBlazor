using System.Linq;
using System.Threading.Tasks;

namespace UpBlazor.Core.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task AddAsync(T model);
        Task UpdateAsync(T model);
        Task AddOrUpdateAsync(T model);
        Task DeleteAsync(T model);

        IQueryable<T> Query();
    }
}