using System.Reflection;

using Dapper;

using MessagingSample.Database.EF;
using MessagingSample.Tests;

using Microsoft.EntityFrameworkCore;

namespace MessagingSample.UnitTests;

[Collection(SharedCollectionDefinition.Name)]
[Trait("Category", "Integration")]
public sealed class DatabaseCreationTests : IDisposable
{
    private readonly TestServices _services;

    public DatabaseCreationTests(DatabaseContainerFixture database)
    {
        _services = new TestServices(database);
    }

    [Fact]
    public void DatabaseIsCreated_after_startup_completes()
    {
        const string sql =
            """
            SELECT t.table_name
            FROM information_schema.tables as t
            WHERE t.table_schema = 'public'
            """;

        var ef = _services.GetRequiredService<AppDbContext>();
        var db = ef.Database.GetDbConnection();
        var results = db.Query<string>(sql).ToList();

        var tableCount = ef.GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Count(e => e.DeclaringType == typeof(AppDbContext));

        Assert.True(tableCount > 0);
        Assert.True(results.Count >= tableCount);
    }

    public void Dispose() => _services.Dispose();
}