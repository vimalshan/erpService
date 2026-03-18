namespace MeetingModule.Infrastructure.Messaging;

public class RabbitMqSettings
{
    public string HostName { get; set; } = "localhost";
    public int Port { get; set; } = 5672;
    public string UserName { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public string VirtualHost { get; set; } = "/";
    public string ExchangeName { get; set; } = "meeting_exchange";
    public string MeetingCreatedQueue { get; set; } = "meeting_created_queue";
    public string MeetingStatusChangedQueue { get; set; } = "meeting_status_changed_queue";
}
