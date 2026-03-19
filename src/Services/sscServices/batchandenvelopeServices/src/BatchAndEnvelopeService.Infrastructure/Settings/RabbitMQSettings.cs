namespace BatchAndEnvelopeService.Infrastructure.Settings;

public class RabbitMQSettings
{
    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 5672;
    public string Username { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public string VirtualHost { get; set; } = "/";
    public string BatchExchange { get; set; } = "batch.exchange";
    public string EnvelopeExchange { get; set; } = "envelope.exchange";
}
