using AccidentManagementService.Infrastructure.EventBus.Integration;
using AccidentManagementService.Domain.Repositories;
using MassTransit;

namespace AccidentManagementService.Infrastructure.EventBus.Consumers;

public class AccidentReportCreatedConsumer : IConsumer<AccidentReportCreatedIntegrationEvent>
{
    private readonly ILogger<AccidentReportCreatedConsumer> _logger;

    public AccidentReportCreatedConsumer(ILogger<AccidentReportCreatedConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<AccidentReportCreatedIntegrationEvent> context)
    {
        var @event = context.Message;
        _logger.LogInformation(
            "Processing AccidentReportCreated: {AccidentNumber} from company {CompanyCode}",
            @event.AccidentNumber, @event.CompanyCode);
        await Task.CompletedTask;
    }
}

public class AccidentStatusChangedConsumer : IConsumer<AccidentStatusChangedIntegrationEvent>
{
    private readonly ILogger<AccidentStatusChangedConsumer> _logger;

    public AccidentStatusChangedConsumer(ILogger<AccidentStatusChangedConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<AccidentStatusChangedIntegrationEvent> context)
    {
        var @event = context.Message;
        _logger.LogInformation(
            "Processing AccidentStatusChanged: {AccidentNumber} from {OldStatusName} to {NewStatusName}",
            @event.AccidentNumber, @event.OldStatusName, @event.NewStatusName);
        await Task.CompletedTask;
    }
}

public class AccidentSeverityChangedConsumer : IConsumer<AccidentSeverityChangedIntegrationEvent>
{
    private readonly ILogger<AccidentSeverityChangedConsumer> _logger;

    public AccidentSeverityChangedConsumer(ILogger<AccidentSeverityChangedConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<AccidentSeverityChangedIntegrationEvent> context)
    {
        var @event = context.Message;
        _logger.LogInformation(
            "Processing AccidentSeverityChanged: {AccidentNumber} from severity {OldSeverityId} to {NewSeverityId}",
            @event.AccidentNumber, @event.OldSeverityId, @event.NewSeverityId);
        await Task.CompletedTask;
    }
}

public class AccidentDetailsUpdatedConsumer : IConsumer<AccidentDetailsUpdatedIntegrationEvent>
{
    private readonly ILogger<AccidentDetailsUpdatedConsumer> _logger;

    public AccidentDetailsUpdatedConsumer(ILogger<AccidentDetailsUpdatedConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<AccidentDetailsUpdatedIntegrationEvent> context)
    {
        var @event = context.Message;
        _logger.LogInformation(
            "Processing AccidentDetailsUpdated: {AccidentNumber}",
            @event.AccidentNumber);
        await Task.CompletedTask;
    }
}

public class AccidentReportDeletedConsumer : IConsumer<AccidentReportDeletedIntegrationEvent>
{
    private readonly ILogger<AccidentReportDeletedConsumer> _logger;

    public AccidentReportDeletedConsumer(ILogger<AccidentReportDeletedConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<AccidentReportDeletedIntegrationEvent> context)
    {
        var @event = context.Message;
        _logger.LogInformation(
            "Processing AccidentReportDeleted: {AccidentNumber}",
            @event.AccidentNumber);
        await Task.CompletedTask;
    }
}

public class AccidentReportRestoredConsumer : IConsumer<AccidentReportRestoredIntegrationEvent>
{
    private readonly ILogger<AccidentReportRestoredConsumer> _logger;

    public AccidentReportRestoredConsumer(ILogger<AccidentReportRestoredConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<AccidentReportRestoredIntegrationEvent> context)
    {
        var @event = context.Message;
        _logger.LogInformation(
            "Processing AccidentReportRestored: {AccidentNumber}",
            @event.AccidentNumber);
        await Task.CompletedTask;
    }
}
