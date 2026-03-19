namespace OrderScheduleService.IntegrationEvents;

public class RabbitMqConfiguration
{
    public string HostName { get; set; } = "localhost";
    public int Port { get; set; } = 5672;
    public string UserName { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public string VirtualHost { get; set; } = "/";
    public string QueueName { get; set; } = "order.schedule.events";
    public string ExchangeName { get; set; } = "order.schedule.exchange";
}

public abstract record IntegrationEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime CreationDate { get; } = DateTime.UtcNow;
}

public record OrderCreatedIntegrationEvent(
    long OrderId,
    string CustomerCode,
    decimal CompanyUnitId,
    DateTime OrderedDate) : IntegrationEvent;

public record OrderScheduledIntegrationEvent(
    long OrderId,
    long DetailId,
    DateTime ScheduledDate,
    long AllocatedQuantity) : IntegrationEvent;

public record OrderCancelledIntegrationEvent(
    long OrderId,
    long DetailId,
    string Reason) : IntegrationEvent;

public record OrderFulfilledIntegrationEvent(
    long OrderId,
    long DetailId,
    long FulfilledQuantity) : IntegrationEvent;

public record ScheduleConfirmedIntegrationEvent(
    DateTime ScheduleDate,
    string Status) : IntegrationEvent;

public record CapacityChangedIntegrationEvent(
    decimal FillingLineId,
    decimal FillingGroupId,
    DateTime ChangeDate) : IntegrationEvent;
