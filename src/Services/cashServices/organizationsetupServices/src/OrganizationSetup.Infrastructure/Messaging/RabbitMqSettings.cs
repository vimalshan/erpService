namespace OrganizationSetup.Infrastructure.Messaging;

public class RabbitMqSettings
{
    public const string SectionName = "RabbitMQ";
    public string HostName { get; set; } = "localhost";
    public int Port { get; set; } = 5672;
    public string UserName { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public string VirtualHost { get; set; } = "/";
    public string ExchangeName { get; set; } = "organization.setup.exchange";
    public string QueueName { get; set; } = "organization.setup.queue";
}
