namespace UtilityService.Infrastructure.Messaging;

public class RabbitMQSettings
{
    public const string SectionName = "RabbitMQ";

    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 5672;
    public string VirtualHost { get; set; } = "/";
    public string Username { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public string ExchangeName { get; set; } = "utility.exchange";
    public string QueueName { get; set; } = "utility.toadplan.events";
    public string DeadLetterExchange { get; set; } = "utility.dlx";
}
