using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace PurchaseOrderService.Infrastructure.Messaging.Consumers;

public record SupplierUpdatedMessage(int SupplierId, string SupplierName, string Status);

public class SupplierUpdatedConsumer : RabbitMqConsumerBase<SupplierUpdatedMessage>
{
    private readonly IServiceProvider _serviceProvider;

    protected override string QueueName => "purchaseorder.supplier.updated";
    protected override string Exchange => "erp.exchange";
    protected override string RoutingKey => "supplier.updated";

    public SupplierUpdatedConsumer(IConfiguration configuration, ILogger<SupplierUpdatedConsumer> logger, IServiceProvider serviceProvider)
        : base(configuration, logger)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task HandleMessageAsync(SupplierUpdatedMessage message, CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<SupplierUpdatedConsumer>>();
        logger.LogInformation("Received supplier updated event for SupplierId: {SupplierId}, Name: {Name}", message.SupplierId, message.SupplierName);
        await Task.CompletedTask;
    }
}
