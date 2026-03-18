using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ReimbursementService.Application.DTOs;

namespace ReimbursementService.Infrastructure.Dapper;

/// <summary>Read-side queries using Dapper for high-performance reporting.</summary>
public sealed class DapperReimbursementReadService(IConfiguration configuration)
{
    private string ConnectionString => configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");

    public async Task<IEnumerable<ReimbursementSummaryDto>> GetSummaryByEmployeeAsync(long empSysId)
    {
        const string sql = """
            SELECT
                EMP_SYSID        AS EmpSysId,
                REIM_TYPE        AS ReimType,
                COUNT(*)         AS Count,
                SUM(REIM_AMOUNT) AS TotalAmount,
                REIM_CURRENCY    AS Currency
            FROM REIM_TRAN
            WHERE REIM_STATUS = 'PAID'
              AND EMP_SYSID = @EmpSysId
            GROUP BY EMP_SYSID, REIM_TYPE, REIM_CURRENCY
            """;

        using var connection = new SqlConnection(ConnectionString);
        return await connection.QueryAsync<ReimbursementSummaryDto>(sql, new { EmpSysId = empSysId });
    }

    public async Task<IEnumerable<dynamic>> GetPendingReimbursementsAsync()
    {
        const string sql = """
            SELECT REIM_ID, REIM_REF_NO, EMP_SYSID, REIM_TYPE, REIM_AMOUNT,
                   REIM_CURRENCY, REIM_STATUS, REIM_DATE
            FROM REIM_TRAN
            WHERE REIM_STATUS IN ('SUBMITTED', 'APPROVED')
            ORDER BY REIM_DATE DESC
            """;

        using var connection = new SqlConnection(ConnectionString);
        return await connection.QueryAsync(sql);
    }

    public async Task<IEnumerable<dynamic>> GetMonthlyReportAsync(int year, int month)
    {
        const string sql = """
            SELECT
                EMP_SYSID,
                REIM_TYPE,
                COUNT(*)         AS ClaimCount,
                SUM(REIM_AMOUNT) AS TotalAmount,
                REIM_CURRENCY
            FROM REIM_TRAN
            WHERE YEAR(REIM_DATE) = @Year
              AND MONTH(REIM_DATE) = @Month
              AND REIM_STATUS = 'PAID'
            GROUP BY EMP_SYSID, REIM_TYPE, REIM_CURRENCY
            ORDER BY TotalAmount DESC
            """;

        using var connection = new SqlConnection(ConnectionString);
        return await connection.QueryAsync(sql, new { Year = year, Month = month });
    }
}
