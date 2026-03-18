namespace EmployeeService.Shared.Messaging;

public class RabbitMqSettings
{
    public string HostName { get; set; } = "localhost";
    public int Port { get; set; } = 5672;
    public string UserName { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public string VirtualHost { get; set; } = "/";
    public string EmployeeEventsQueueName { get; set; } = "employee.events";
    public int PublishRetryCount { get; set; } = 2;
    public int PublishRetryDelaySeconds { get; set; } = 2;
    public int PublishCircuitBreakDurationSeconds { get; set; } = 30;
    public int PublishCircuitMinimumThroughput { get; set; } = 2;
    public double PublishCircuitFailureRatio { get; set; } = 0.5;
    public int ConsumerRetryDelaySeconds { get; set; } = 10;
}