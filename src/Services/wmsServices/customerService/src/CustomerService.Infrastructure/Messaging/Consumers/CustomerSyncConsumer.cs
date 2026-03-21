using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CustomerService.Infrastructure.Messaging.Consumers;

public sealed record CustomerSyncMessage(int CustomerId, string Code, string Name, string Action);

public class CustomerSyncConsumer : RabbitMqConsumerBase<CustomerSyncMessage>
{
    protected override string QueueName => "customer.sync.queue";
    protected override string ExchangeName => "customer.exchange";
    protected override string RoutingKey => "customer.sync.#";

    public CustomerSyncConsumer(IConfiguration configuration, ILogger<CustomerSyncConsumer> logger)
        : base(configuration, logger)
    {
    }

    protected override Task HandleMessageAsync(CustomerSyncMessage message, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Processing customer sync: {CustomerId} - {Action}", message.CustomerId, message.Action);
        // Process synchronization logic here
        return Task.CompletedTask;
    }
}
