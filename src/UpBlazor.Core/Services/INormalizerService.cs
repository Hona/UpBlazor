using System.Threading.Tasks;

namespace UpBlazor.Core.Services
{
    public interface INormalizerService
    {
        Task UpdateUserAsync(string userId);
    }
}