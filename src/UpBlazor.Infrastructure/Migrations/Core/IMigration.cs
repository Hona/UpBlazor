using System.Threading.Tasks;
using Marten;

namespace UpBlazor.Infrastructure.Migrations.Core;

public interface IMigration
{
    int Version { get; }
    Task MigrateAsync(IDocumentStore store);
}