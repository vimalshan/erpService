using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Dapper;
using Microsoft.Data.SqlClient;

namespace LeaveServices.Functions;

/// <summary>
/// Timer-triggered function that accrues monthly leave credits for all employees.
/// Runs on the 1st of every month at 00:00 UTC.
/// </summary>
public sealed class LeaveBalanceAccrualFunction
{
    private readonly ILogger<LeaveBalanceAccrualFunction> _logger;
    private readonly string _connectionString;

    public LeaveBalanceAccrualFunction(
        ILogger<LeaveBalanceAccrualFunction> logger,
        IConfiguration configuration)
    {
        _logger           = logger;
        _connectionString = configuration["SqlConnectionString"]
            ?? throw new InvalidOperationException("SqlConnectionString not configured.");
    }

    [Function(nameof(LeaveBalanceAccrualFunction))]
    public async Task Run(
        [TimerTrigger("0 0 0 1 * *")] TimerInfo timerInfo,
        CancellationToken ct)
    {
        _logger.LogInformation("LeaveBalanceAccrual started at {Time}", DateTime.UtcNow);

        using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync(ct);

        // Example: credit 1.5 days/month of CL (LEAVE_ID=1) for every active employee
        var employees = await conn.QueryAsync<long>(
            "SELECT DISTINCT CREDIT_EMPSYSID FROM LEAVE_CREDIT WHERE CREDIT_YEAR = @Year",
            new { Year = DateTime.UtcNow.Year });

        int count = 0;
        foreach (var empId in employees)
        {
            var nextId = await conn.ExecuteScalarAsync<long>(
                "SELECT ISNULL(MAX(CREDIT_ID),0)+1 FROM LEAVE_CREDIT");
            await conn.ExecuteAsync(
                """
                INSERT INTO LEAVE_CREDIT
                (CREDIT_ID,CREDIT_EMPSYSID,CREDIT_LEAVEID,CREDIT_LEAVEFLAG,CREDIT_YEAR,
                 CREDIT_OPENING,CREDIT_CREDITED,CREDIT_UTILIZED,CREDIT_CLOSING,
                 CREDIT_LASTMODIFIEDBY,CREDIT_LASTMODIFIEDON)
                VALUES (@Id,@Emp,1,'M',@Year,0,1.5,0,1.5,1,GETDATE())
                """,
                new { Id = nextId, Emp = empId, Year = DateTime.UtcNow.Year });
            count++;
        }

        _logger.LogInformation("LeaveBalanceAccrual credited {Count} employee(s).", count);
    }
}

/// <summary>
/// Timer-triggered function that sends notifications for pending leave approvals.
/// Runs every day at 08:00 UTC.
/// </summary>
public sealed class LeaveNotificationFunction
{
    private readonly ILogger<LeaveNotificationFunction> _logger;
    private readonly string _connectionString;

    public LeaveNotificationFunction(
        ILogger<LeaveNotificationFunction> logger,
        IConfiguration configuration)
    {
        _logger           = logger;
        _connectionString = configuration["SqlConnectionString"]
            ?? throw new InvalidOperationException("SqlConnectionString not configured.");
    }

    [Function(nameof(LeaveNotificationFunction))]
    public async Task Run(
        [TimerTrigger("0 0 8 * * *")] TimerInfo timerInfo,
        CancellationToken ct)
    {
        _logger.LogInformation("LeaveNotification started at {Time}", DateTime.UtcNow);

        using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync(ct);

        var pending = await conn.QueryAsync<PendingLeaveDto>(
            """
            SELECT ld.LEAVE_DETAILID AS LeaveDetailId,
                   ld.LEAVE_EMPSYSID AS EmpSysId,
                   lm.LEAVE_DESCRIPTION AS LeaveType,
                   ld.LEAVE_APPFROM AS AppFrom,
                   ld.LEAVE_APPTO AS AppTo,
                   ld.LEAVE_APPLIEDDAYS AS AppliedDays
            FROM LEAVE_DETAILS ld
            INNER JOIN LEAVE_MASTER lm ON lm.LEAVE_ID = ld.LEAVE_ID
            WHERE ld.LEAVE_APPSTATUS = 'P'
            """);

        foreach (var item in pending)
            _logger.LogInformation(
                "PENDING APPROVAL – DetailId={Id}, Emp={Emp}, Type={Type}, Days={Days}",
                item.LeaveDetailId, item.EmpSysId, item.LeaveType, item.AppliedDays);
    }

    private record PendingLeaveDto(
        long     LeaveDetailId,
        long     EmpSysId,
        string   LeaveType,
        DateTime AppFrom,
        DateTime AppTo,
        decimal  AppliedDays);
}
