using LookupService.Application.Commands;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LookupService.Infrastructure.Messaging.Consumers;

public record LovSyncMessage(string LovType, string LovName);

public class LovSyncConsumer(
    IConfiguration configuration,
    ILogger<LovSyncConsumer> logger,
    IServiceScopeFactory scopeFactory)
    : RabbitMqConsumerBase<LovSyncMessage>(configuration, logger, "lookup.lov.sync", "lookup.exchange", "lov.sync")
{
    protected override async Task HandleMessageAsync(LovSyncMessage message, CancellationToken ct)
    {
        using var scope = scopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        await mediator.Send(new CreateLovCommand(message.LovType, message.LovName), ct);
        logger.LogInformation("Synced LOV: {LovType} - {LovName}", message.LovType, message.LovName);
    }
}

public record ProcessSyncMessage(decimal ProcessId, string ProcessName, string LiveFlag);

public class ProcessSyncConsumer(
    IConfiguration configuration,
    ILogger<ProcessSyncConsumer> logger,
    IServiceScopeFactory scopeFactory)
    : RabbitMqConsumerBase<ProcessSyncMessage>(configuration, logger, "lookup.process.sync", "lookup.exchange", "process.sync")
{
    protected override async Task HandleMessageAsync(ProcessSyncMessage message, CancellationToken ct)
    {
        using var scope = scopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        await mediator.Send(new CreateProcessCommand(message.ProcessId, message.ProcessName, message.LiveFlag), ct);
        logger.LogInformation("Synced Process: {ProcessName}", message.ProcessName);
    }
}
