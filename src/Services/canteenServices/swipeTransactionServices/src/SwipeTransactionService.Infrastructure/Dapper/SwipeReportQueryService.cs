using Dapper;
using Microsoft.Data.SqlClient;
using SwipeTransactionService.Application.DTOs;

namespace SwipeTransactionService.Infrastructure.Dapper;

public sealed class SwipeReportQueryService
{
    private readonly string _connectionString;

    public SwipeReportQueryService(string connectionString) => _connectionString = connectionString;

    public async Task<IEnumerable<SwipeUploadSummaryDto>> GetSummaryByBatchAsync(
        long batchNumber,
        CancellationToken ct = default)
    {
        const string sql = """
            SELECT
                CN_EMP_NUM   AS EmployeeNumber,
                CN_SWP_TIM   AS SwipeTime,
                CN_ITM_COD   AS ItemCode,
                CN_ITM_QTN   AS Quantity,
                CASE CN_UPD_STS
                    WHEN 'Y' THEN 'Processed'
                    WHEN 'F' THEN 'Failed'
                    ELSE 'Pending'
                END AS Status
            FROM CANTEEN_SWIPE_CARD_UPLOAD
            WHERE CN_BAT_NUM = @BatchNumber
            ORDER BY CN_SWP_TIM
            """;

        await using var conn = new SqlConnection(_connectionString);
        var cmd = new CommandDefinition(sql, new { BatchNumber = batchNumber }, cancellationToken: ct);
        return await conn.QueryAsync<SwipeUploadSummaryDto>(cmd);
    }

    public async Task<IEnumerable<DailyAvailedDto>> GetDailyAvailedByEmployeeAsync(
        long empSysId,
        string date,
        CancellationToken ct = default)
    {
        const string sql = """
            SELECT
                CN_SRL_NUM   AS SerialNumber,
                CN_COM_COD   AS CompanyCode,
                CN_SYS_ID    AS EmployeeSysId,
                CN_ITM_COD   AS ItemCode,
                CN_EE_CON    AS EmployeeContribution,
                CN_ER_CON    AS EmployerContribution,
                CN_ITM_QTY   AS ItemQuantity,
                CN_SWP_DAT   AS SwipeDate,
                CN_CAN_NUM   AS CanteenNumber
            FROM CANTEEN_DAYWISE_AVAILED
            WHERE CN_SYS_ID = @EmpSysId AND CN_SWP_DAT = @Date
            ORDER BY CN_SRL_NUM
            """;

        await using var conn = new SqlConnection(_connectionString);
        var cmd = new CommandDefinition(sql, new { EmpSysId = empSysId, Date = date }, cancellationToken: ct);
        return await conn.QueryAsync<DailyAvailedDto>(cmd);
    }
}
