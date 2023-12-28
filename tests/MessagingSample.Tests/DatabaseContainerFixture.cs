using System.Data;
using System.Diagnostics;

using MessagingSample.Database;
using MessagingSample.Tests;

using Npgsql;

namespace MessagingSample.UnitTests;

using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Networks;

public sealed class DatabaseContainerFixture : IAsyncLifetime, IDisposable
{
    private const string PostgresNames = "test";

    private readonly CancellationTokenSource _cts = CancellationTokenSourceFactory.Create(30);

    #pragma warning disable CA2213

    private readonly INetwork _network;
    private readonly IContainer _dbContainer;

    #pragma warning restore CA2213

    public DatabaseContainerFixture()
    {
        var cleanup = !Debugger.IsAttached;

        _network = new NetworkBuilder()
            .WithName(Guid.NewGuid().ToString("D"))
            .WithCleanUp(true)
            .Build();

        _dbContainer = new ContainerBuilder()
            .WithImage("postgres")
            .WithNetwork(_network)
            .WithName("MessagingSample.Tests.db")
            .WithNetworkAliases("db")
            .WithVolumeMount("postgres-data", "/var/lib/postgresql/data")
            .WithEnvironment("POSTGRES_USER", PostgresNames)
            .WithEnvironment("POSTGRES_PASSWORD", PostgresNames)
            .WithEnvironment("POSTGRES_DB", PostgresNames)
            .WithPortBinding(5432, 5432)
            .WithCleanUp(cleanUp: cleanup)
            .Build();
    }

    public static string ConnectionString
        => $"Host=localhost;Database={PostgresNames};Username={PostgresNames};Password={PostgresNames}";

    public async Task InitializeAsync()
    {
        await _network.CreateAsync(_cts.Token)
            .ConfigureAwait(false);

        await _dbContainer.StartAsync(_cts.Token)
            .ConfigureAwait(false);

        using var services = new TestServices(this);
        var sut = services.GetRequiredService<DatabaseCreationService>();
        await WaitUntilConnectAvailableAsync(30).ConfigureAwait(false);
        using var ct = CancellationTokenSourceFactory.Create(10);
        await sut.StartAsync(ct.Token).ConfigureAwait(false);
        var task = sut.ExecuteTask;
        if (task is not null)
        {
            await task.ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Asynchronously attempts to connect to the database engine until the connection
    /// is available. When creating a container for the first time, there may be some delay due
    /// to creation of the volume mount point and execution of the container startup script. Just
    /// attempting to connect is sufficient for detecting that the database engine inside the
    /// container is "ready," as the entire creation process will succeed from the time that the
    /// connection becomes available.
    /// </summary>
    /// <param name="timeout">
    /// Specifies a timeout in seconds. When a debugger is attached, this will be minutes.
    /// </param>
    private async Task WaitUntilConnectAvailableAsync(int timeout)
    {
        var cancellation = CancellationTokenSourceFactory.Create(timeout);
        using var connection = new NpgsqlConnection(ConnectionString);
        int retryCount = 0;
        Connect:
        while (connection.State != ConnectionState.Open)
        {
            try
            {
                await connection.OpenAsync(cancellation.Token).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Debugger.Break();

                if (retryCount++ < 5)
                {
                    await Task.Delay(1000 * retryCount, cancellation.Token).ConfigureAwait(false);
                    goto Connect;
                }

                throw;
            }
        }
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _cts.Dispose();
    }
}