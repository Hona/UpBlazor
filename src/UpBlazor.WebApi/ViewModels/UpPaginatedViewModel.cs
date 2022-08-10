using Up.NET.Models;

namespace UpBlazor.WebApi.ViewModels;

public class UpPaginatedViewModel<T>
{
    public List<T>? Data { get; set; }
    public PaginatedLinks? Links { get; set; }
}