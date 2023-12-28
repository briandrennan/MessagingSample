using Microsoft.Extensions.DependencyInjection;
using MessagingSample.Database;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using Serilog.Extensions.Logging;
using MessagingSample.UnitTests;

namespace MessagingSample.Tests;

public sealed class TestServices : IDisposable
{
    private readonly IServiceCollection _services;
    private readonly IServiceProvider _serviceProvider;
    private readonly IServiceScope _scope;

    public TestServices(
        DatabaseContainerFixture fixture,
        Action<IServiceCollection>? configureCallback = null)
    {
        ArgumentNullException.ThrowIfNull(fixture);

        IServiceCollection services = new ServiceCollection();
        services.AddLogging(options =>
        {
            options.AddProvider(NullLoggerProvider.Instance);
        });
        services.AddAppDatabaseServices(DatabaseContainerFixture.ConnectionString);
        configureCallback?.Invoke(services);

        _services = services;
        _serviceProvider = _services.BuildServiceProvider();
        _scope = _serviceProvider.CreateScope();
    }

    public TService? GetService<TService>() => _scope.ServiceProvider.GetService<TService>();

    public TService GetRequiredService<TService>() where TService : notnull
        => _scope.ServiceProvider.GetRequiredService<TService>();

    public void Dispose()
    {
        _scope.Dispose();
    }
}