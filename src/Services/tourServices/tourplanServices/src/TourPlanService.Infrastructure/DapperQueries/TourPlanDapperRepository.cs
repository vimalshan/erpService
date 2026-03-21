using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TourPlanService.Application.DTOs;

namespace TourPlanService.Infrastructure.DapperQueries;

public sealed class TourPlanDapperRepository(IConfiguration configuration)
{
    private SqlConnection CreateConnection() =>
        new(configuration.GetConnectionString("TourPlanDb"));

    public async Task<IEnumerable<TourPlanSummaryDto>> GetTourPlansByStatusAsync(
        string status, CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT TP_ID AS TpId, TP_EMPSYSID AS TpEmpSysId,
                   TP_STARTDATE AS TpStartDate, TP_ENDDATE AS TpEndDate,
                   TP_PURPOSE AS TpPurpose, TP_STATUS AS TpStatus,
                   TP_CATEGORY AS TpCategory, TP_FROMCITYNAME AS TpFromCityName,
                   TP_TOCITYNAME AS TpToCityName, TP_CREATEDON AS TpCreatedOn
            FROM TOURPLAN_MAIN
            WHERE TP_STATUS = @Status
            ORDER BY TP_CREATEDON DESC
            """;

        await using var connection = CreateConnection();
        return await connection.QueryAsync<TourPlanSummaryDto>(sql, new { Status = status });
    }

    public async Task<IEnumerable<TourPlanSummaryDto>> GetPendingApprovalTourPlansAsync(
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT TP_ID AS TpId, TP_EMPSYSID AS TpEmpSysId,
                   TP_STARTDATE AS TpStartDate, TP_ENDDATE AS TpEndDate,
                   TP_PURPOSE AS TpPurpose, TP_STATUS AS TpStatus,
                   TP_CATEGORY AS TpCategory, TP_FROMCITYNAME AS TpFromCityName,
                   TP_TOCITYNAME AS TpToCityName, TP_CREATEDON AS TpCreatedOn
            FROM TOURPLAN_MAIN
            WHERE TP_STATUS IN ('SUBMITTED', 'PENDING')
            ORDER BY TP_CREATEDON ASC
            """;

        await using var connection = CreateConnection();
        return await connection.QueryAsync<TourPlanSummaryDto>(sql);
    }

    public async Task<TourAdvanceDto?> GetAdvanceByTourPlanIdAsync(
        string tpId, CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT ADV_ID AS AdvId, ADV_TPID AS AdvTpId,
                   ADV_AMOUNT AS AdvAmount, ADV_JVID AS AdvJvId,
                   ADV_REMARKS AS AdvRemarks, ADV_APPSTATUS AS AdvAppStatus,
                   ADV_APPBY AS AdvAppBy, ADV_APPON AS AdvAppOn,
                   ADV_CURRENCY AS AdvCurrency, ADV_RATE AS AdvRate,
                   ADV_TOTAL AS AdvTotal, ADV_TYPE AS AdvType,
                   ADV_PAYMODE AS AdvPayMode
            FROM TOURPLAN_ADVANCE
            WHERE ADV_TPID = @TpId
            """;

        await using var connection = CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<TourAdvanceDto>(sql, new { TpId = tpId });
    }

    public async Task<IEnumerable<ForexRequisitionDto>> GetForexByTourPlanIdAsync(
        string tpId, CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT FORREQ_ID AS ForReqId, FORREQ_TPID AS ForReqTpId,
                   FORREQ_PASSNO AS ForReqPassNo, FORREQ_PASSNAME AS ForReqPassName,
                   FORREQ_PASSLOCATION AS ForReqPassLocation,
                   FORREQ_PASSEXPDATE AS ForReqPassExpDate,
                   FORREQ_DESTINATION AS ForReqDestination,
                   FORREQ_STATUS AS ForReqStatus, FORREQ_TYPE AS ForReqType,
                   FORREQ_TOTVALUE AS ForReqTotValue, FORREQ_CURRENCY AS ForReqCurrency
            FROM TOURPLAN_FOREXMAIN
            WHERE FORREQ_TPID = @TpId
            """;

        await using var connection = CreateConnection();
        return await connection.QueryAsync<ForexRequisitionDto>(sql, new { TpId = tpId });
    }
}
