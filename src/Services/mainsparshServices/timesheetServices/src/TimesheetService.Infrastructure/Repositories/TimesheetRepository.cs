using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TimesheetService.Domain.Entities;
using TimesheetService.Domain.Interfaces;
using TimesheetService.Domain.ValueObjects;
using TimesheetService.Infrastructure.Data;

namespace TimesheetService.Infrastructure.Repositories;

public sealed class TimesheetRepository : ITimesheetRepository
{
    private readonly TimesheetDbContext _context;
    private readonly string _connectionString;

    public TimesheetRepository(TimesheetDbContext context, IConfiguration configuration)
    {
        _context          = context;
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
    }

    // EF Core — single entity operations
    public async Task<Timesheet?> GetByIdAsync(long timesheetId, CancellationToken cancellationToken = default)
        => await _context.Timesheets.AsNoTracking()
                         .FirstOrDefaultAsync(t => t.TimesheetId == timesheetId, cancellationToken);

    public async Task<Timesheet> AddAsync(Timesheet timesheet, CancellationToken cancellationToken = default)
    {
        await _context.Timesheets.AddAsync(timesheet, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return timesheet;
    }

    public async Task UpdateAsync(Timesheet timesheet, CancellationToken cancellationToken = default)
    {
        _context.Timesheets.Update(timesheet);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(long timesheetId, CancellationToken cancellationToken = default)
        => await _context.Timesheets.AnyAsync(t => t.TimesheetId == timesheetId, cancellationToken);

    public async Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default)
        => await _context.Timesheets.CountAsync(cancellationToken);

    // ── Dapper ── optimised list reads ─────────────────────────────────────
    public async Task<IEnumerable<Timesheet>> GetByEmployeeIdAsync(
        long employeeId, DateOnly? from = null, DateOnly? to = null,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT TIMESHEET_ID, EMP_SYSID, TIMESHEET_DATE, WORK_DATE,
                   START_TIME, END_TIME, TOTAL_HOURS, PROJECT_ID, TASK_ID,
                   WORK_DESCRIPTION, RECORDED_DATE, TIMESHEET_STATUS, APPROVAL_STATUS,
                   APPROVED_BY, APPROVED_ON, REJECTION_REASON,
                   CREATED_BY, CREATED_ON, UPDATED_BY, UPDATED_ON
            FROM TSE_TIMESHEET
            WHERE EMP_SYSID = @EmployeeId
              AND (@From IS NULL OR WORK_DATE >= @From)
              AND (@To   IS NULL OR WORK_DATE <= @To)
            ORDER BY WORK_DATE DESC
            """;

        var rows = await QueryDapperAsync<TimesheetDapperRow>(sql, new
        {
            EmployeeId = employeeId,
            From       = from.HasValue ? (object)from.Value.ToDateTime(TimeOnly.MinValue) : DBNull.Value,
            To         = to.HasValue   ? (object)to.Value.ToDateTime(TimeOnly.MinValue)   : DBNull.Value
        }, cancellationToken);

        return rows.Select(MapToEntity);
    }

    public async Task<IEnumerable<Timesheet>> GetPendingTimesheetsAsync(CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT TIMESHEET_ID, EMP_SYSID, TIMESHEET_DATE, WORK_DATE,
                   START_TIME, END_TIME, TOTAL_HOURS, PROJECT_ID, TASK_ID,
                   WORK_DESCRIPTION, RECORDED_DATE, TIMESHEET_STATUS, APPROVAL_STATUS,
                   APPROVED_BY, APPROVED_ON, REJECTION_REASON,
                   CREATED_BY, CREATED_ON, UPDATED_BY, UPDATED_ON
            FROM TSE_TIMESHEET
            WHERE APPROVAL_STATUS = 'PENDING'
              AND TIMESHEET_STATUS = 'SUBMITTED'
            ORDER BY RECORDED_DATE DESC
            """;

        var rows = await QueryDapperAsync<TimesheetDapperRow>(sql, null, cancellationToken);
        return rows.Select(MapToEntity);
    }

    public async Task<IEnumerable<Timesheet>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT TIMESHEET_ID, EMP_SYSID, TIMESHEET_DATE, WORK_DATE,
                   START_TIME, END_TIME, TOTAL_HOURS, PROJECT_ID, TASK_ID,
                   WORK_DESCRIPTION, RECORDED_DATE, TIMESHEET_STATUS, APPROVAL_STATUS,
                   APPROVED_BY, APPROVED_ON, REJECTION_REASON,
                   CREATED_BY, CREATED_ON, UPDATED_BY, UPDATED_ON
            FROM TSE_TIMESHEET
            ORDER BY CREATED_ON DESC
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY
            """;

        var rows = await QueryDapperAsync<TimesheetDapperRow>(sql,
            new { Offset = (pageNumber - 1) * pageSize, PageSize = pageSize }, cancellationToken);
        return rows.Select(MapToEntity);
    }

    // ── helpers ─────────────────────────────────────────────────────────────
    private async Task<IEnumerable<T>> QueryDapperAsync<T>(string sql, object? param, CancellationToken ct)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(ct);
        return await connection.QueryAsync<T>(sql, param, commandTimeout: 30);
    }

    private static Timesheet MapToEntity(TimesheetDapperRow r)
    {
        var t = Timesheet.Create(
            r.EMP_SYSID,
            DateOnly.FromDateTime(r.TIMESHEET_DATE),
            DateOnly.FromDateTime(r.WORK_DATE),
            r.START_TIME.HasValue ? TimeOnly.FromTimeSpan(r.START_TIME.Value) : null,
            r.END_TIME.HasValue   ? TimeOnly.FromTimeSpan(r.END_TIME.Value)   : null,
            r.TOTAL_HOURS,
            r.PROJECT_ID,
            r.TASK_ID,
            r.WORK_DESCRIPTION,
            r.CREATED_BY);

        // Suppress events raised by the factory on the read path
        t.ClearDomainEvents();
        return t;
    }

    // Dapper read model — column names match the SQL schema exactly
    private sealed class TimesheetDapperRow
    {
        public long     TIMESHEET_ID     { get; set; }
        public long     EMP_SYSID        { get; set; }
        public DateTime TIMESHEET_DATE   { get; set; }
        public DateTime WORK_DATE        { get; set; }
        public TimeSpan?START_TIME       { get; set; }
        public TimeSpan?END_TIME         { get; set; }
        public decimal? TOTAL_HOURS      { get; set; }
        public long?    PROJECT_ID       { get; set; }
        public long?    TASK_ID          { get; set; }
        public string?  WORK_DESCRIPTION { get; set; }
        public DateTime RECORDED_DATE    { get; set; }
        public string   TIMESHEET_STATUS { get; set; } = "DRAFT";
        public string   APPROVAL_STATUS  { get; set; } = "PENDING";
        public long?    APPROVED_BY      { get; set; }
        public DateTime?APPROVED_ON      { get; set; }
        public string?  REJECTION_REASON { get; set; }
        public long     CREATED_BY       { get; set; }
        public DateTime CREATED_ON       { get; set; }
        public long?    UPDATED_BY       { get; set; }
        public DateTime?UPDATED_ON       { get; set; }
    }
}
