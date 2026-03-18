using AccidentManagementService.Infrastructure.EventBus;
using MassTransit;

namespace AccidentManagementService.Infrastructure.EventConsumers;

/// <summary>
/// Consumer for AccidentReportCreatedIntegrationEvent
/// Handles all actions that should occur when a new accident report is created
/// Examples: Send notifications, trigger workflows, log to audit system
/// </summary>
public class AccidentReportCreatedConsumer : IConsumer<AccidentReportCreatedIntegrationEvent>
{
    private readonly ILogger<AccidentReportCreatedConsumer> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public AccidentReportCreatedConsumer(
        ILogger<AccidentReportCreatedConsumer> logger,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Consumes (handles) the AccidentReportCreatedIntegrationEvent
    /// </summary>
    public async Task Consume(ConsumeContext<AccidentReportCreatedIntegrationEvent> context)
    {
        var @event = context.Message;

        _logger.LogInformation(
            "AccidentReportCreatedConsumer: Processing new accident report {AccidentNumber} " +
            "from company {CompanyCode} at {CreatedTime}",
            @event.AccidentNumber,
            @event.CompanyCode,
            @event.CreatedTime);

        try
        {
            // TODO: Implement business logic for accident creation
            // Examples:
            // 1. Send email notification to health & safety team
            // 2. Create follow-up task for investigation
            // 3. Log to compliance/audit system
            // 4. Trigger SLA notification (if it's a critical accident)
            // 5. Update external systems (e.g., insurance provider)
            // 6. Publish to analytics/reporting system

            // Example implementation:
            await ProcessAccidentCreationAsync(@event);

            _logger.LogInformation(
                "AccidentReportCreatedConsumer: Successfully processed accident {AccidentNumber}",
                @event.AccidentNumber);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "AccidentReportCreatedConsumer: Error processing accident creation event " +
                "for accident {AccidentNumber}",
                @event.AccidentNumber);

            // Re-throw to trigger retry by message bus
            throw;
        }
    }

    private async Task ProcessAccidentCreationAsync(AccidentReportCreatedIntegrationEvent @event)
    {
        // Task 1: Log audit trail
        // var auditEntry = new AuditLog
        // {
        //     EventType = "AccidentCreated",
        //     TableName = "DAILY_ACC_FIR",
        //     EntityId = @event.AccidentReportId,
        //     AccidentNumber = @event.AccidentNumber,
        //     Changes = JsonConvert.SerializeObject(@event),
        //     CreatedDate = DateTime.UtcNow,
        //     CreatedBy = "System"
        // };
        // await _unitOfWork.AuditLogs.AddAsync(auditEntry);
        // await _unitOfWork.SaveAsync();

        // Task 2: Send notification
        // await SendNotificationEmailAsync(@event);

        // Task 3: Check if it's a critical accident and create task
        // if (@event.SeverityId == 1) // Critical
        // {
        //     var task = new Task
        //     {
        //         Title = $"Critical Accident Investigation: {@event.AccidentNumber}",
        //         Description = $"A critical accident was reported at {@event.CompanyCode}",
        //         Priority = Priority.High,
        //         DueDate = DateTime.UtcNow.AddDays(1),
        //         AssignedTo = "HealthSafetyManager"
        //     };
        // }

        await Task.CompletedTask;
    }

    // private async Task SendNotificationEmailAsync(AccidentReportCreatedIntegrationEvent @event)
    // {
    //     var emailTemplate = $@"
    //         <h2>New Accident Report Created</h2>
    //         <p><strong>Accident Number:</strong> {@event.AccidentNumber}</p>
    //         <p><strong>Company:</strong> {@event.CompanyCode}</p>
    //         <p><strong>Created:</strong> {@event.CreatedTime:yyyy-MM-dd HH:mm:ss}</p>
    //         <p>Please review and take appropriate action.</p>
    //     ";

    //     // var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
    //     // await emailService.SendEmailAsync(
    //     //     to: "health-safety@company.com",
    //     //     subject: $"New Accident Report: {@event.AccidentNumber}",
    //     //     body: emailTemplate,
    //     //     isHtml: true);
    // }
}

/// <summary>
/// Consumer for AccidentStatusChangedIntegrationEvent
/// Handles status transitions (New → InProgress → Resolved → Closed)
/// </summary>
public class AccidentStatusChangedConsumer : IConsumer<AccidentStatusChangedIntegrationEvent>
{
    private readonly ILogger<AccidentStatusChangedConsumer> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public AccidentStatusChangedConsumer(
        ILogger<AccidentStatusChangedConsumer> logger,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task Consume(ConsumeContext<AccidentStatusChangedIntegrationEvent> context)
    {
        var @event = context.Message;

        _logger.LogInformation(
            "AccidentStatusChangedConsumer: Accident {AccidentNumber} status changed " +
            "from {OldStatusId} ({OldStatusName}) to {NewStatusId} ({NewStatusName})",
            @event.AccidentNumber,
            @event.OldStatusId,
            @event.OldStatusName,
            @event.NewStatusId,
            @event.NewStatusName);

        try
        {
            await ProcessStatusChangeAsync(@event);

            _logger.LogInformation(
                "AccidentStatusChangedConsumer: Successfully processed status change for {AccidentNumber}",
                @event.AccidentNumber);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "AccidentStatusChangedConsumer: Error processing status change for {AccidentNumber}",
                @event.AccidentNumber);
            throw;
        }
    }

    private async Task ProcessStatusChangeAsync(AccidentStatusChangedIntegrationEvent @event)
    {
        // Route processing based on new status
        switch (@event.NewStatusId)
        {
            case 1: // New
                // Initial notification
                _logger.LogInformation("Processing NEW status for {@event.AccidentNumber}");
                break;

            case 2: // InProgress
                // Start investigation workflow
                // Assign investigator
                // Set investigation deadline
                _logger.LogInformation("Starting investigation for {@event.AccidentNumber}");
                break;

            case 3: // Resolved
                // Complete investigation
                // Generate incident report
                // Update follow-up items
                _logger.LogInformation("Finalizing investigation for {@event.AccidentNumber}");
                break;

            case 4: // Closed
                // Archive records
                // Complete all follow-ups
                // Send closure notification
                _logger.LogInformation("Closing incident {@event.AccidentNumber}");
                break;

            default:
                _logger.LogWarning("Unknown status {StatusId} for accident {@event.AccidentNumber}",
                    @event.NewStatusId, @event.AccidentNumber);
                break;
        }

        await Task.CompletedTask;
    }
}

/// <summary>
/// Consumer for AccidentSeverityChangedIntegrationEvent
/// Handles severity level changes that may affect investigation approach
/// </summary>
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
            "AccidentSeverityChangedConsumer: Accident {AccidentNumber} severity changed " +
            "from {OldSeverityId} to {NewSeverityId}",
            @event.AccidentNumber,
            @event.OldSeverityId,
            @event.NewSeverityId);

        try
        {
            // If severity increased to Critical, escalate priority
            if (@event.NewSeverityId == 1) // Critical
            {
                _logger.LogWarning(
                    "ALERT: Accident {AccidentNumber} escalated to CRITICAL severity. Escalating to management.",
                    @event.AccidentNumber);
                // Send urgent notification
                // Escalate to senior management
            }
            // If severity decreased, adjust investigation scope
            else if (@event.NewSeverityId == 4) // Low
            {
                _logger.LogInformation(
                    "Accident {AccidentNumber} downgraded to LOW severity. Reducing investigation scope.",
                    @event.AccidentNumber);
            }

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "AccidentSeverityChangedConsumer: Error processing severity change for {AccidentNumber}",
                @event.AccidentNumber);
            throw;
        }
    }
}

/// <summary>
/// Consumer for AccidentDetailsUpdatedIntegrationEvent
/// Tracks updates to accident information during investigation
/// </summary>
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
            "AccidentDetailsUpdatedConsumer: Accident {AccidentNumber} details updated",
            @event.AccidentNumber);

        try
        {
            // TODO: Log to audit trail
            // Track what was changed
            // Notify interested parties if changes are significant
            
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "AccidentDetailsUpdatedConsumer: Error processing details update for {AccidentNumber}",
                @event.AccidentNumber);
            throw;
        }
    }
}

/// <summary>
/// Consumer for AccidentReportDeletedIntegrationEvent
/// Handles cleanup when an accident report is soft-deleted
/// </summary>
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
            "AccidentReportDeletedConsumer: Accident {AccidentNumber} marked as deleted",
            @event.AccidentNumber);

        try
        {
            // TODO: Archive related documents
            // Mark follow-up tasks as cancelled
            // Log to audit trail with reason for deletion
            
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "AccidentReportDeletedConsumer: Error processing deletion for {AccidentNumber}",
                @event.AccidentNumber);
            throw;
        }
    }
}

/// <summary>
/// Consumer for AccidentReportRestoredIntegrationEvent
/// Handles restoration of soft-deleted accident reports
/// </summary>
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
            "AccidentReportRestoredConsumer: Accident {AccidentNumber} has been restored",
            @event.AccidentNumber);

        try
        {
            // TODO: Restore related documents from archive
            // Notify stakeholders of restoration
            // Log audit trail entry
            
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "AccidentReportRestoredConsumer: Error processing restoration for {AccidentNumber}",
                @event.AccidentNumber);
            throw;
        }
    }
}

/// <summary>
/// Consumer for InjuryCategoryCreatedIntegrationEvent
/// Handles new injury category master data
/// </summary>
public class InjuryCategoryCreatedConsumer : IConsumer<InjuryCategoryCreatedIntegrationEvent>
{
    private readonly ILogger<InjuryCategoryCreatedConsumer> _logger;

    public InjuryCategoryCreatedConsumer(ILogger<InjuryCategoryCreatedConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<InjuryCategoryCreatedIntegrationEvent> context)
    {
        var @event = context.Message;

        _logger.LogInformation(
            "InjuryCategoryCreatedConsumer: New injury category created: {CategoryName}",
            @event.CategoryName);

        try
        {
            // Notify any subscribed services about new category
            // Potentially update caches/materialized views
            
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "InjuryCategoryCreatedConsumer: Error processing new category");
            throw;
        }
    }
}

/// <summary>
/// Consumer for InjuryNatureCreatedIntegrationEvent
/// Handles new injury nature master data
/// </summary>
public class InjuryNatureCreatedConsumer : IConsumer<InjuryNatureCreatedIntegrationEvent>
{
    private readonly ILogger<InjuryNatureCreatedConsumer> _logger;

    public InjuryNatureCreatedConsumer(ILogger<InjuryNatureCreatedConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<InjuryNatureCreatedIntegrationEvent> context)
    {
        var @event = context.Message;

        _logger.LogInformation(
            "InjuryNatureCreatedConsumer: New injury nature created: {NatureName}",
            @event.NatureName);

        try
        {
            // Notify any subscribed services about new nature
            
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "InjuryNatureCreatedConsumer: Error processing new nature");
            throw;
        }
    }
}
