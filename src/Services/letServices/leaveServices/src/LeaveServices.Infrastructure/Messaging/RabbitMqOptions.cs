namespace LeaveServices.Infrastructure.Messaging;

public sealed class RabbitMqOptions
{
    public const string SectionName = "RabbitMQ";

    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 5672;
    public string Username { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public string VirtualHost { get; set; } = "/";
    public string LeaveExchange { get; set; } = "leave.events";
    public string EncashmentQueue { get; set; } = "leave.encashment.events";
    public string LopQueue { get; set; } = "leave.lop.events";
}
