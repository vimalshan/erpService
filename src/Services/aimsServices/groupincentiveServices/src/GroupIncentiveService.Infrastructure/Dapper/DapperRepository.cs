using Dapper;
using GroupIncentiveService.Application.DTOs;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace GroupIncentiveService.Infrastructure.Dapper;

public interface IDapperRepository
{
    Task<EmployeeIncentiveSummaryDto?> GetEmployeeIncentiveSummaryAsync(long employeeId, int month, int year, CancellationToken ct = default);
    Task<IEnumerable<GroupIncentiveMainDto>> GetGroupIncentivesByStatusAsync(string status, CancellationToken ct = default);
    Task<decimal> GetGroupTotalIncentiveAsync(int groupId, int month, int year, CancellationToken ct = default);
}

public class DapperRepository : IDapperRepository
{
    private readonly string _connectionString;

    public DapperRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection string not configured.");
    }

    public async Task<EmployeeIncentiveSummaryDto?> GetEmployeeIncentiveSummaryAsync(
        long employeeId, int month, int year, CancellationToken ct = default)
    {
        const string sql = """
            SELECT 
                gid.GRPINCDET_EMPSYSID AS EmployeeId,
                gim.GRPINC_INCMONTH AS Month,
                gim.GRPINC_INCYEAR AS Year,
                SUM(gid.GRPINCDET_ALLOCAMOUNT) AS TotalAllocatedAmount,
                SUM(ISNULL(gid.GRPINCDET_APPROVEDAMOUNT, 0)) AS TotalApprovedAmount
            FROM GROUPINCENTIVE_DET gid
            INNER JOIN GROUPINCENTIVE_MAIN gim ON gid.GRPINCDET_MAINID = gim.GRPINC_ID
            WHERE gid.GRPINCDET_EMPSYSID = @EmployeeId
              AND gim.GRPINC_INCMONTH = @Month
              AND gim.GRPINC_INCYEAR = @Year
            GROUP BY gid.GRPINCDET_EMPSYSID, gim.GRPINC_INCMONTH, gim.GRPINC_INCYEAR
            """;

        using var connection = new SqlConnection(_connectionString);
        var result = await connection.QueryFirstOrDefaultAsync<EmployeeIncentiveSummaryDto>(
            new CommandDefinition(sql, new { EmployeeId = employeeId, Month = month, Year = year },
                cancellationToken: ct));
        return result;
    }

    public async Task<IEnumerable<GroupIncentiveMainDto>> GetGroupIncentivesByStatusAsync(
        string status, CancellationToken ct = default)
    {
        const string sql = """
            SELECT 
                m.GRPINC_ID AS GrpIncId,
                m.GRPINC_GROUPID AS GrpIncGroupId,
                g.GROUP_NAME AS GroupName,
                m.GRPINC_INCMONTH AS GrpIncIncMonth,
                m.GRPINC_INCYEAR AS GrpIncIncYear,
                m.GRPINC_TOTALAMOUNT AS GrpIncTotalAmount,
                m.GRPINC_APPSTATUS AS GrpIncAppStatus,
                m.GRPINC_APPROVEDAMOUNT AS GrpIncApprovedAmount,
                m.GRPINC_APPROVER AS GrpIncApprover,
                m.GRPINC_APPROVALDATE AS GrpIncApprovalDate,
                m.GRPINC_ENTEREDON AS GrpIncEnteredOn,
                m.GRPINC_ENTEREDBY AS GrpIncEnteredBy
            FROM GROUPINCENTIVE_MAIN m
            INNER JOIN Group_Master g ON m.GRPINC_GROUPID = g.GROUP_ID
            WHERE m.GRPINC_APPSTATUS = @Status
            ORDER BY m.GRPINC_ENTEREDON DESC
            """;

        using var connection = new SqlConnection(_connectionString);
        var results = await connection.QueryAsync<GroupIncentiveMainDto>(
            new CommandDefinition(sql, new { Status = status }, cancellationToken: ct));
        return results;
    }

    public async Task<decimal> GetGroupTotalIncentiveAsync(int groupId, int month, int year, CancellationToken ct = default)
    {
        const string sql = "SELECT dbo.fn_GetGroupTotalIncentive(@GroupId, @Month, @Year)";
        using var connection = new SqlConnection(_connectionString);
        return await connection.ExecuteScalarAsync<decimal>(
            new CommandDefinition(sql, new { GroupId = groupId, Month = month, Year = year },
                cancellationToken: ct));
    }
}
