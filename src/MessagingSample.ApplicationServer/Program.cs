using MassTransit;

using MessagingSample;
using MessagingSample.Database.EF;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSerilogAppLogging("ApplicationServer");

var connectionString = builder.Configuration.GetConnectionString("AppDB")
                       ?? throw new InvalidOperationException("Connection string missing.");
Console.WriteLine("AppDB connectionString: {0}", connectionString);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

var rabbitSettings = builder.Configuration.GetRabbitMqSettings(sectionName: "RabbitMQ");
builder.Services.AddMassTransit(bus =>
{
    bus.UsingRabbitMq((context, rabbit) =>
    {
        rabbit.Host(rabbitSettings.HostName, rabbitSettings.VHost, settings =>
        {
            settings.MaxMessageSize(8192);
            settings.Heartbeat(TimeSpan.FromSeconds(30));
            settings.Username(rabbitSettings.UserName);
            settings.Password(rabbitSettings.Password);
        });
    });
});

builder.Services.AddHealthChecks()
    .AddNpgSql(name: "AppDB", connectionString: connectionString, tags: new[] { "db", "postgres" });

var app = builder.Build();

// NOTE: you wouldn't normally expose these in a public-facing app.
app.UseSwagger();
app.UseSwaggerUI();

app.MapHealthChecks("/hc", new HealthCheckOptions
{
    ResponseWriter = HealthChecks.UI.Client.UIResponseWriter.WriteHealthCheckUIResponse
});

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
