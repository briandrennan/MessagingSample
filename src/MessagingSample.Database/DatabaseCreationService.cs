using MessagingSample.Database.EF;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MessagingSample.Database;

public class DatabaseCreationService : BackgroundService
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<DatabaseCreationService> _logger;

    public DatabaseCreationService(
        AppDbContext dbContext,
        ILogger<DatabaseCreationService> logger)
    {
        ArgumentNullException.ThrowIfNull(dbContext);
        ArgumentNullException.ThrowIfNull(logger);

        _dbContext = dbContext;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var result = await _dbContext.Database.EnsureCreatedAsync(stoppingToken).ConfigureAwait(false);

        _logger.LogInformation("Database creation for connection string '{ConnectionString}' returned result {Result}.",
            _dbContext.Database.GetConnectionString(), result);
    }
}