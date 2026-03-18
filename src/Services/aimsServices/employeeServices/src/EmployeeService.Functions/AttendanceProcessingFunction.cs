using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using EmployeeService.Infrastructure.Dapper;

namespace EmployeeService.Functions;

/// <summary>
/// Timer-triggered Azure Function that runs every hour to process
/// pending attendance records and flag inconsistencies.
/// </summary>
public sealed class AttendanceProcessingFunction
{
    private readonly DapperService _dapper;
    private readonly ILogger<AttendanceProcessingFunction> _logger;

    public AttendanceProcessingFunction(DapperService dapper, ILogger<AttendanceProcessingFunction> logger)
    {
        _dapper = dapper;
        _logger = logger;
    }

    [Function(nameof(AttendanceProcessingFunction))]
    public async Task Run(
        [TimerTrigger("0 0 * * * *")] TimerInfo timer,  // every hour
        FunctionContext context,
        CancellationToken ct)
    {
        _logger.LogInformation("AttendanceProcessingFunction triggered at {Time}", DateTime.UtcNow);

        const string sql = @"
            SELECT TIME_INFOID, TIME_EMPSYSID, TIME_EMPATTFLAG
            FROM EMP_TIMEINFO
            WHERE TIME_LASTMODIFIEDON >= DATEADD(HOUR, -1, GETDATE())";

        var records = await _dapper.QueryAsync<dynamic>(sql, ct: ct);
        _logger.LogInformation("Processed {Count} time-info records.", records.Count());
    }
}

/// <summary>
/// Timer-triggered function that archives old log records daily.
/// </summary>
public sealed class LogArchivalFunction
{
    private readonly DapperService _dapper;
    private readonly ILogger<LogArchivalFunction> _logger;

    public LogArchivalFunction(DapperService dapper, ILogger<LogArchivalFunction> logger)
    {
        _dapper = dapper;
        _logger = logger;
    }

    [Function(nameof(LogArchivalFunction))]
    public async Task Run(
        [TimerTrigger("0 0 2 * * *")] TimerInfo timer,  // 2 AM daily
        FunctionContext context,
        CancellationToken ct)
    {
        _logger.LogInformation("LogArchivalFunction triggered at {Time}", DateTime.UtcNow);

        // Purge log records older than 90 days
        const string sql = @"
            DELETE FROM EMP_TIMEINFO_LOG
            WHERE LOG_CREATEBY < DATEADD(DAY, -90, GETDATE())";

        var deleted = await _dapper.ExecuteAsync(sql, ct: ct);
        _logger.LogInformation("Archived {Count} stale time-info log records.", deleted);
    }
}
