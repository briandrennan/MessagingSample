using MessagingSample.Database.EF;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MessagingSample.Database;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAppDatabaseServices(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(connectionString)
                .UseSnakeCaseNamingConvention();
        });

        services.AddSingleton<DatabaseCreationService>();

        return services;
    }
}