using IntegrationService.Application.DTOs;
using IntegrationService.Application.PurchaseOrders.Commands;
using IntegrationService.Application.Vendors.Commands;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IntegrationService.Infrastructure.Messaging.Consumers;

public class PurchaseOrderSyncConsumer(
    IConfiguration configuration,
    ILogger<PurchaseOrderSyncConsumer> logger,
    IServiceProvider serviceProvider)
    : RabbitMqConsumerBase<PurchaseOrderDto>(configuration, logger)
{
    protected override string QueueName => "integration.po.sync";
    protected override string ExchangeName => "integration";
    protected override string RoutingKey => "po.sync";

    protected override async Task HandleMessageAsync(PurchaseOrderDto message, CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var command = new CreatePurchaseOrderCommand(
            message.PoSeqId, message.OracleOrgId, message.OraclePoId,
            message.PoNumber, message.VendorSiteId,
            message.DueDays, message.DueDayMonthOffset, message.MonthForward);

        await mediator.Send(command, cancellationToken);
        logger.LogInformation("Synced PO {PoNumber} from message queue", message.PoNumber);
    }
}

public class VendorSyncConsumer(
    IConfiguration configuration,
    ILogger<VendorSyncConsumer> logger,
    IServiceProvider serviceProvider)
    : RabbitMqConsumerBase<VendorDto>(configuration, logger)
{
    protected override string QueueName => "integration.vendor.sync";
    protected override string ExchangeName => "integration";
    protected override string RoutingKey => "vendor.sync";

    protected override async Task HandleMessageAsync(VendorDto message, CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var command = new CreateVendorCommand(
            message.VendorId, message.VendorName, message.VendorCode);

        await mediator.Send(command, cancellationToken);
        logger.LogInformation("Synced Vendor {VendorName} from message queue", message.VendorName);
    }
}
