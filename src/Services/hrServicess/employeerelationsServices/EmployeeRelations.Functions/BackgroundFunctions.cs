using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using EmployeeRelations.Infrastructure.Persistence.Dapper;

namespace EmployeeRelations.Functions;

/// <summary>Timer-triggered function that closes EWS periods past their due date.</summary>
public class EwsPeriodCloseFunction
{
    private readonly IDapperReadRepository _dapperRepo;
    private readonly ILogger<EwsPeriodCloseFunction> _logger;

    public EwsPeriodCloseFunction(IDapperReadRepository dapperRepo, ILogger<EwsPeriodCloseFunction> logger)
    {
        _dapperRepo = dapperRepo;
        _logger = logger;
    }

    /// <summary>Runs daily at midnight to evaluate EWS period closure.</summary>
    [Function(nameof(EwsPeriodCloseFunction))]
    public async Task RunAsync([TimerTrigger("0 0 0 * * *")] TimerInfo timerInfo, CancellationToken ct)
    {
        _logger.LogInformation("EWS Period Close function executing at {Time}", DateTime.UtcNow);
        // In production: query open periods past close date, mark them closed, send notifications
        await Task.CompletedTask;
    }
}

/// <summary>Timer-triggered function that sends EWS reminder notifications.</summary>
public class EwsReminderFunction
{
    private readonly ILogger<EwsReminderFunction> _logger;

    public EwsReminderFunction(ILogger<EwsReminderFunction> logger) => _logger = logger;

    /// <summary>Runs every Monday at 9 AM to send EWS pending reminders.</summary>
    [Function(nameof(EwsReminderFunction))]
    public async Task RunAsync([TimerTrigger("0 0 9 * * MON")] TimerInfo timerInfo, CancellationToken ct)
    {
        _logger.LogInformation("EWS Reminder function executing at {Time}", DateTime.UtcNow);
        // In production: query pending EWS records and trigger email/push notifications
        await Task.CompletedTask;
    }
}

/// <summary>Timer-triggered function that auto-locks surveys past their end date.</summary>
public class SurveyAutoLockFunction
{
    private readonly ILogger<SurveyAutoLockFunction> _logger;

    public SurveyAutoLockFunction(ILogger<SurveyAutoLockFunction> logger) => _logger = logger;

    /// <summary>Runs daily at 1 AM to auto-lock eligible surveys.</summary>
    [Function(nameof(SurveyAutoLockFunction))]
    public async Task RunAsync([TimerTrigger("0 0 1 * * *")] TimerInfo timerInfo, CancellationToken ct)
    {
        _logger.LogInformation("Survey AutoLock function executing at {Time}", DateTime.UtcNow);
        // In production: query surveys with AutoLock=Y past EndDate, mark as closed
        await Task.CompletedTask;
    }
}
