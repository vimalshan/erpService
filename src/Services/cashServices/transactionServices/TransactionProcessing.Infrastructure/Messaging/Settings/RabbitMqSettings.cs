namespace TransactionProcessing.Infrastructure.Messaging.Settings;

public sealed class RabbitMqSettings
{
    public string HostName { get; set; } = "localhost";
    public int Port { get; set; } = 5672;
    public string UserName { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public string VirtualHost { get; set; } = "/";
    public string ExchangeName { get; set; } = "transaction-processing-exchange";
    public string QueueName { get; set; } = "transaction-processing-queue";
}
