using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ExitManagement.Functions;

/// <summary>
/// Timer-triggered Azure Function to check for overdue exit formalities.
/// Runs daily at 08:00 UTC.
/// </summary>
public class ExitFormalityReminderFunction
{
    private readonly ILogger<ExitFormalityReminderFunction> _logger;

    public ExitFormalityReminderFunction(ILogger<ExitFormalityReminderFunction> logger)
        => _logger = logger;

    [Function(nameof(ExitFormalityReminderFunction))]
    public void Run([TimerTrigger("0 0 8 * * *")] TimerInfo timerInfo)
    {
        _logger.LogInformation("[AzureFunc] ExitFormalityReminderFunction triggered at {Time}", DateTime.UtcNow);

        // TODO: Query exits where formality is pending and relieve date is approaching.
        // Inject IMediator or IEmployeeExitRepository and process.
        // Send reminder emails or push notifications.

        if (timerInfo.ScheduleStatus is not null)
        {
            _logger.LogInformation("[AzureFunc] Next timer schedule: {Next}", timerInfo.ScheduleStatus.Next);
        }
    }
}

/// <summary>
/// Timer-triggered Azure Function to clean up expired exit documents from blob storage.
/// Runs monthly on the 1st at midnight UTC.
/// </summary>
public class ExitDocumentCleanupFunction
{
    private readonly ILogger<ExitDocumentCleanupFunction> _logger;

    public ExitDocumentCleanupFunction(ILogger<ExitDocumentCleanupFunction> logger)
        => _logger = logger;

    [Function(nameof(ExitDocumentCleanupFunction))]
    public void Run([TimerTrigger("0 0 0 1 * *")] TimerInfo timerInfo)
    {
        _logger.LogInformation("[AzureFunc] ExitDocumentCleanupFunction triggered at {Time}", DateTime.UtcNow);

        // TODO: List and delete blob documents older than retention period.
        // Use IBlobStorageService from Application layer.
    }
}

/// <summary>
/// Timer-triggered Azure Function to auto-archive completed exits older than 2 years.
/// Runs annually on Jan 1st at midnight UTC.
/// </summary>
public class ExitArchiveFunction
{
    private readonly ILogger<ExitArchiveFunction> _logger;

    public ExitArchiveFunction(ILogger<ExitArchiveFunction> logger)
        => _logger = logger;

    [Function(nameof(ExitArchiveFunction))]
    public void Run([TimerTrigger("0 0 0 1 1 *")] TimerInfo timerInfo)
    {
        _logger.LogInformation("[AzureFunc] ExitArchiveFunction triggered at {Time}", DateTime.UtcNow);

        // TODO: Archive exits older than 2 years to cold storage or archive table.
    }
}
