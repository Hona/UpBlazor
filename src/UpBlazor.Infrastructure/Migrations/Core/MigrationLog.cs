using System;

namespace UpBlazor.Infrastructure.Migrations.Core;

public class MigrationLog
{
    public int Version { get; set; }
    public DateTime Timestamp { get; set; }
}