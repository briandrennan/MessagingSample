using Microsoft.Extensions.DependencyInjection;

using Serilog;

namespace MessagingSample;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddSerilogAppLogging(this IServiceCollection services, string serviceName)
    {
        ArgumentException.ThrowIfNullOrEmpty(serviceName);

        services.AddLogging(logging =>
        {
            var logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.WithProperty("ServiceName", serviceName)
                .WriteTo.Console()
                .CreateLogger();

            logging.AddSerilog(logger);
        });
        return services;
    }
}
