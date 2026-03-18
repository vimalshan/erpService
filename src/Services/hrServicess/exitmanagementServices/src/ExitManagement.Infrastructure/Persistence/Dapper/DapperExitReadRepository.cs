using Dapper;
using ExitManagement.Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ExitManagement.Infrastructure.Persistence.Dapper;

/// <summary>
/// Dapper-based read repository for complex/reporting queries on exit data.
/// </summary>
public class DapperExitReadRepository
{
    private readonly string _connectionString;

    public DapperExitReadRepository(IConfiguration configuration)
        => _connectionString = configuration.GetConnectionString("DefaultConnection")
           ?? throw new InvalidOperationException("DefaultConnection string is not configured.");

    public async Task<IEnumerable<dynamic>> GetExitSummaryAsync(CancellationToken ct = default)
    {
        const string sql = @"
            SELECT e.EXIT_NO         AS ExitNo,
                   e.EXIT_EMP_SYSID  AS EmployeeSysId,
                   e.EXIT_STATUS     AS Status,
                   e.EXIT_RES_TYPE   AS ResignationType,
                   e.EXIT_EXP_RELDT  AS ExpectedRelieveDate,
                   e.EXIT_APPSTATUS  AS ApprovalStatus
            FROM   TTBT_EXIT_TEV e
            ORDER  BY e.EXIT_NO DESC";

        using var conn = new SqlConnection(_connectionString);
        return await conn.QueryAsync(new CommandDefinition(sql, cancellationToken: ct));
    }

    public async Task<dynamic?> GetExitDetailAsync(decimal exitNo, CancellationToken ct = default)
    {
        const string sql = @"
            SELECT e.*,
                   i.INT_QUES_ID AS QuestionId,
                   i.INT_FEEDBACK AS Feedback
            FROM   TTBT_EXIT_TEV e
            LEFT JOIN EMPLOYEE_EXIT_INT i ON i.INT_EXITNO = e.EXIT_NO
            WHERE  e.EXIT_NO = @ExitNo";

        using var conn = new SqlConnection(_connectionString);
        return await conn.QueryFirstOrDefaultAsync(new CommandDefinition(sql, new { ExitNo = exitNo }, cancellationToken: ct));
    }

    public async Task<IEnumerable<dynamic>> GetExitsByStatusAsync(string status, CancellationToken ct = default)
    {
        const string sql = @"
            SELECT EXIT_NO, EXIT_EMP_SYSID, EXIT_STATUS, EXIT_EXP_RELDT
            FROM   TTBT_EXIT_TEV
            WHERE  EXIT_STATUS = @Status
            ORDER  BY EXIT_LET_GIVON DESC";

        using var conn = new SqlConnection(_connectionString);
        return await conn.QueryAsync(new CommandDefinition(sql, new { Status = status }, cancellationToken: ct));
    }
}
