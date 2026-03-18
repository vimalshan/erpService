using EmployeeManagement.Application.Common.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Functions.Functions;

/// <summary>
/// Timer-triggered Azure Function that generates a daily headcount report
/// and uploads it to Azure Blob Storage.
/// </summary>
public class EmployeeReportFunction(IDapperQueryService dapperQueryService,
    IBlobStorageService blobStorageService,
    ILogger<EmployeeReportFunction> logger)
{
    // Runs every day at 07:00 UTC
    [Function(nameof(EmployeeReportFunction))]
    public async Task Run([TimerTrigger("0 0 7 * * *")] TimerInfo timerInfo)
    {
        logger.LogInformation("EmployeeReportFunction triggered at: {Time}", DateTimeOffset.UtcNow);

        try
        {
            const string sql = """
                SELECT
                    D.DEPT_NAME,
                    COUNT(C.CAREER_EMP_SYSID)  AS HeadCount,
                    SUM(CASE WHEN E.EMP_MARITALSTATUS = 'M' THEN 1 ELSE 0 END) AS Married,
                    SUM(CASE WHEN E.EMP_GENDER        = 'F' THEN 1 ELSE 0 END) AS Female
                FROM  EMPLOYEE_CAREER    C
                JOIN  EMP_DEPARTMENT     D ON D.DEPT_SYSID = C.CAREER_DEPT_SYSID
                JOIN  EMPLOYEEMASTER     E ON E.EMP_SYSID  = C.CAREER_EMP_SYSID
                WHERE C.CAREER_TO IS NULL
                GROUP BY D.DEPT_NAME
                ORDER BY D.DEPT_NAME
                """;

            var rows = await dapperQueryService.QueryAsync<DeptHeadcount>(sql, null);

            var csv = BuildCsv(rows);
            var fileName = $"reports/headcount-{DateTimeOffset.UtcNow:yyyyMMdd}.csv";

            await using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(csv));
            await blobStorageService.UploadAsync("employee-reports", fileName, stream, "text/csv");

            logger.LogInformation("EmployeeReportFunction: report uploaded to {FileName}", fileName);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in EmployeeReportFunction");
            throw;
        }
    }

    private static string BuildCsv(IEnumerable<DeptHeadcount> rows)
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine("Department,HeadCount,Married,Female");
        foreach (var r in rows)
            sb.AppendLine($"{r.DeptName},{r.HeadCount},{r.Married},{r.Female}");
        return sb.ToString();
    }

    private sealed record DeptHeadcount(string DeptName, int HeadCount, int Married, int Female);
}
