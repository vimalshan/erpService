using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CustomerService.Infrastructure.Messaging.Consumers;

public sealed record CustomerNotificationMessage(int CustomerId, string Email, string Subject, string Body);

public class CustomerNotificationConsumer : RabbitMqConsumerBase<CustomerNotificationMessage>
{
    protected override string QueueName => "customer.notification.queue";
    protected override string ExchangeName => "customer.exchange";
    protected override string RoutingKey => "customer.notification.#";

    public CustomerNotificationConsumer(IConfiguration configuration, ILogger<CustomerNotificationConsumer> logger)
        : base(configuration, logger)
    {
    }

    protected override Task HandleMessageAsync(CustomerNotificationMessage message, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Sending notification to {Email} for customer {CustomerId}", message.Email, message.CustomerId);
        // Process notification logic here
        return Task.CompletedTask;
    }
}
