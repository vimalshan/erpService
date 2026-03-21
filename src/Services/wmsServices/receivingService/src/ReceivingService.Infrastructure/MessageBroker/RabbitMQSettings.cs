namespace ReceivingService.Infrastructure.MessageBroker;

public sealed class RabbitMQSettings
{
    public const string SectionName = "RabbitMQ";
    public string Host     { get; set; } = "localhost";
    public int    Port     { get; set; } = 5672;
    public string UserName { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public string ExchangeName { get; set; } = "receiving.exchange";
}
