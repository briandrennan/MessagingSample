using Microsoft.Extensions.Configuration;

namespace MessagingSample;

public static class ConfigurationExtensions
{
    public static RabbitMqSettings GetRabbitMqSettings(this IConfiguration configuration, string? sectionName = null)
    {
        if (sectionName is not null)
        {
            configuration = configuration.GetSection(sectionName);
        }

        return configuration.Get<RabbitMqSettings>() ?? throw new InvalidOperationException("Failed to bind the RabbitMQ configuration.");
    }
}