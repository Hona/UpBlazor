namespace UpBlazor.Infrastructure.EfCore
{
    /// <summary>
    /// What type of DBs we support.
    /// We can add CosmosDB, PostgresSQL, In-Memory and lots more.
    /// </summary>
    public enum DbType
    {
        SqlServer,
        Sqlite,
        SqliteInMemory
    }
}
