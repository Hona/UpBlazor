using System.Threading.Tasks;

namespace UpBlazor.Application.Services
{
    public interface INormalizerService
    {
        Task UpdateUserAsync(string userId);
    }
}