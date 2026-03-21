using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace PurchaseOrderService.Infrastructure.Messaging.Consumers;

public record InventoryReceivedMessage(int ProductId, decimal Quantity, string WarehouseCode);

public class InventoryReceivedConsumer : RabbitMqConsumerBase<InventoryReceivedMessage>
{
    private readonly IServiceProvider _serviceProvider;

    protected override string QueueName => "purchaseorder.inventory.received";
    protected override string Exchange => "erp.exchange";
    protected override string RoutingKey => "inventory.received";

    public InventoryReceivedConsumer(IConfiguration configuration, ILogger<InventoryReceivedConsumer> logger, IServiceProvider serviceProvider)
        : base(configuration, logger)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task HandleMessageAsync(InventoryReceivedMessage message, CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<InventoryReceivedConsumer>>();
        logger.LogInformation("Received inventory received event for ProductId: {ProductId}, Qty: {Quantity}", message.ProductId, message.Quantity);
        await Task.CompletedTask;
    }
}
