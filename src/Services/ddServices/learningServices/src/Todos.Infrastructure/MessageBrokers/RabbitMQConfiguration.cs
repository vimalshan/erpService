namespace Todos.Infrastructure.MessageBrokers;

/// <summary>
/// Configuration for RabbitMQ
/// </summary>
public class RabbitMQConfiguration
{
    public string? HostName { get; set; }
    public int Port { get; set; } = 5672;
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public string? VirtualHost { get; set; } = "/";
    public int PrefetchCount { get; set; } = 1;
    public int ConnectionRetryCount { get; set; } = 3;
    public int ConnectionRetryDelay { get; set; } = 1000;
}
