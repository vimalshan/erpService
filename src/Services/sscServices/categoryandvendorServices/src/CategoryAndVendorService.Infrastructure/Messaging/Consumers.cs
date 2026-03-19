using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using CategoryAndVendorService.Application.VendorDocuments.Commands;

namespace CategoryAndVendorService.Infrastructure.Messaging;

public class VendorDocumentApprovalConsumer : RabbitMqConsumerBase
{
    public VendorDocumentApprovalConsumer(
        IServiceScopeFactory scopeFactory,
        ILogger<VendorDocumentApprovalConsumer> logger,
        string hostName, string userName, string password)
        : base(hostName, userName, password, "vendor-document-approval", scopeFactory, logger)
    {
    }

    protected override async Task HandleMessageAsync(string message, IServiceProvider serviceProvider, CancellationToken ct)
    {
        var command = JsonSerializer.Deserialize<ApproveVendorDocumentCommand>(message);
        if (command is null) return;
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        await mediator.Send(command, ct);
    }
}

public class CategorySyncConsumer : RabbitMqConsumerBase
{
    public CategorySyncConsumer(
        IServiceScopeFactory scopeFactory,
        ILogger<CategorySyncConsumer> logger,
        string hostName, string userName, string password)
        : base(hostName, userName, password, "category-sync", scopeFactory, logger)
    {
    }

    protected override async Task HandleMessageAsync(string message, IServiceProvider serviceProvider, CancellationToken ct)
    {
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        // Deserialize and handle category sync messages
        var logger = serviceProvider.GetRequiredService<ILogger<CategorySyncConsumer>>();
        logger.LogInformation("Received category sync message: {Message}", message);
    }
}
