using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ScholarshipService.Infrastructure.DapperRepositories;

/// <summary>Dapper-based read repository for complex queries and stored procedure calls.</summary>
public class ScholarshipDapperRepository(IConfiguration configuration)
{
    private SqlConnection CreateConnection()
        => new(configuration.GetConnectionString("ScholarshipDb"));

    public async Task<IEnumerable<dynamic>> GetScholarshipSummaryAsync(int? employeeSysId = null)
    {
        const string sql = """
            SELECT sm.SCH_ID, sm.SCH_CHILDNAME, sm.SCH_COURSENAME,
                   sm.SCH_ENTRYSTATUS, sm.SCH_LIVESTATUS, sm.SCH_CREATEDON,
                   COUNT(sd.SCHDET_ID) AS TotalYears,
                   SUM(sd.SCHDET_PAYAMOUNT) AS TotalPaid
            FROM SCHOLARSHIP_MAIN sm
            LEFT JOIN SCHOLARSHIP_DETAIL sd ON sd.SCHDET_MAINID = sm.SCH_ID
            WHERE (@EmployeeSysId IS NULL OR sm.SCH_EMPSYSID = @EmployeeSysId)
            GROUP BY sm.SCH_ID, sm.SCH_CHILDNAME, sm.SCH_COURSENAME,
                     sm.SCH_ENTRYSTATUS, sm.SCH_LIVESTATUS, sm.SCH_CREATEDON
            ORDER BY sm.SCH_CREATEDON DESC
            """;

        await using var conn = CreateConnection();
        return await conn.QueryAsync(sql, new { EmployeeSysId = employeeSysId });
    }

    public async Task<long> GetEligibleAmountAsync(string gradeCat, string eligibleExam, int year)
    {
        await using var conn = CreateConnection();
        return await conn.ExecuteScalarAsync<long>(
            "SELECT dbo.fn_GetScholarshipEligibleAmount(@GradeCat, @EligibleExam, @Year)",
            new { GradeCat = gradeCat, EligibleExam = eligibleExam, Year = year });
    }

    public async Task<int> CreateScholarshipViaSpAsync(CreateScholarshipSpParams p)
    {
        await using var conn = CreateConnection();
        var param = new DynamicParameters(p);
        param.Add("@p_NewSchID", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
        await conn.ExecuteAsync("dbo.usp_ScholarshipApplication", param, commandType: System.Data.CommandType.StoredProcedure);
        return param.Get<int>("@p_NewSchID");
    }

    public async Task ApproveScholarshipViaSpAsync(int scholarshipId, int approvedBy, string? remarks)
    {
        await using var conn = CreateConnection();
        await conn.ExecuteAsync("dbo.usp_ScholarshipApprove",
            new { p_SCH_ID = scholarshipId, p_ApprovedBy = approvedBy, p_AppRemarks = remarks },
            commandType: System.Data.CommandType.StoredProcedure);
    }
}

public record CreateScholarshipSpParams(
    int p_SCH_EMPSYSID, int p_SCH_GRADEID, int p_SCH_DEPENDID, string p_SCH_CHILDNAME,
    string p_SCH_LASTSCHOOL, decimal p_SCH_LASTYEAROFSCHOOL, string p_SCH_LASTEXAM,
    string p_SCH_CGPAFLAG, decimal p_SCH_MARKSPER, decimal p_SCH_MARKSGPA,
    string p_SCH_MARKSFILE, string p_SCH_COURSENAME, int p_SCH_COURSEJOINYEAR,
    decimal p_SCH_COURSEJOINMONTH, long p_SCH_COURSEDURATION,
    string? p_SCH_ADMRECPTFILE, string? p_SCH_PAYMODE, string? p_SCH_CHILDACCNO,
    string? p_SCH_CHILLDBANKIFSC, string? p_SCH_CHILLDBANKMICR,
    string p_SCH_ENTRYSTATUS, string p_SCH_SOURCE, decimal p_SCH_DISBAMOUNT,
    string p_SCH_DISBFREQ, string p_SCH_LIVESTATUS, int p_CreatedBy,
    string p_SCH_OFFLINE, int? p_SCH_OFFLINEYEAR);
