using TaskTransactional.Application.Commands;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TaskTransactional.Infrastructure.Messaging.Consumers;

public record ComplaintSyncMessage(
    string UnitCode, string GroupId, string GroupName, decimal GroupSrc);

public class ComplaintSyncConsumer(
    IConfiguration configuration,
    ILogger<ComplaintSyncConsumer> logger,
    IServiceScopeFactory scopeFactory)
    : RabbitMqConsumerBase<ComplaintSyncMessage>(configuration, logger, "complaint.sync", "complaint.exchange", "complaint.sync")
{
    protected override async Task HandleMessageAsync(ComplaintSyncMessage message, CancellationToken ct)
    {
        using var scope = scopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        await mediator.Send(new CreateComplaintMainCommand(
            message.UnitCode, message.GroupId, message.GroupName, message.GroupSrc), ct);
        logger.LogInformation("Synced complaint: {GroupId} - {GroupName}", message.GroupId, message.GroupName);
    }
}

public record TicketSyncMessage(
    decimal GroupId, decimal Type, decimal Location, decimal Department,
    decimal Process, string TargetDate, string? Subject, string? Description);

public class TicketSyncConsumer(
    IConfiguration configuration,
    ILogger<TicketSyncConsumer> logger,
    IServiceScopeFactory scopeFactory)
    : RabbitMqConsumerBase<TicketSyncMessage>(configuration, logger, "complaint.ticket.sync", "complaint.exchange", "ticket.sync")
{
    protected override async Task HandleMessageAsync(TicketSyncMessage message, CancellationToken ct)
    {
        using var scope = scopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        await mediator.Send(new CreateTicketCommand(
            message.GroupId, message.Type, message.Location, message.Department,
            message.Process, message.TargetDate, message.Subject, message.Description), ct);
        logger.LogInformation("Synced ticket for group: {GroupId}", message.GroupId);
    }
}
