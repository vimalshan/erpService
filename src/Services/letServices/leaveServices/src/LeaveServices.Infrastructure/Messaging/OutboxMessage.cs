namespace LeaveServices.Infrastructure.Messaging;

public sealed class OutboxMessage
{
    public long Id { get; set; }
    public string EventType { get; set; } = default!;
    public string RoutingKey { get; set; } = default!;
    public string Payload { get; set; } = default!;
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public DateTime? ProcessedOn { get; set; }
    public int RetryCount { get; set; }
    public string? Error { get; set; }
}
