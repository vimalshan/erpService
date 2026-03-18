using CurrencyManagement.Application.Common.Interfaces;

namespace CurrencyManagement.Infrastructure.Messaging;

/// <summary>
/// No-op message publisher for when RabbitMQ is not available
/// </summary>
public class NoOpMessagePublisher : IMessagePublisher
{
    public async Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class
    {
        // Do nothing - this is a placeholder for when RabbitMQ is unavailable
        await Task.CompletedTask;
    }
}
