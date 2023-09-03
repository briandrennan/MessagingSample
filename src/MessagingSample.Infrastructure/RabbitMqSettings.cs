namespace MessagingSample;

public class RabbitMqSettings
{
    public required string HostName { get; set; }

    public string VHost { get; set; } = "/";

    public string UserName { get; set; } = "guest";

    public string Password { get; set; } = "guest";
}