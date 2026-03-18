using Dapper;
using DealTicketing.Application.DTOs;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DealTicketing.Infrastructure.ReadRepositories;

/// <summary>Dapper-based read-only queries for high-performance reporting.</summary>
public class DealDapperReadRepository(IConfiguration configuration)
{
    private SqlConnection CreateConnection()
        => new(configuration.GetConnectionString("DealTicketingDb"));

    public async Task<IEnumerable<DealSummaryDto>> GetDealSummaryAsync(
        DateTime fromDate, DateTime toDate, CancellationToken ct = default)
    {
        const string sql = """
            SELECT
                db.DEAL_DATE        AS DealDate,
                bm.BANK_NAME        AS BankName,
                COUNT(*)            AS DealCount,
                SUM(dd.DEAL_AMOUNT) AS TotalAmount,
                COUNT(CASE WHEN dd.DEAL_APPSTATUS = 'Y' THEN 1 END) AS ConfirmedDeals,
                COUNT(CASE WHEN dd.DEAL_APPSTATUS = 'P' THEN 1 END) AS PendingDeals,
                COUNT(CASE WHEN dd.DEAL_APPSTATUS = 'R' THEN 1 END) AS RejectedDeals
            FROM DEALTICKET_BATCH db
            JOIN DEAL_BANKMASTER bm ON db.DEAL_BANKID = bm.BANK_ID
            JOIN DEALTICKET_DET dd ON db.DEAL_BATCHID = dd.DEAL_BATCHID
            WHERE db.DEAL_DATE BETWEEN @FromDate AND @ToDate
            GROUP BY db.DEAL_DATE, bm.BANK_NAME
            ORDER BY db.DEAL_DATE DESC
            """;

        await using var conn = CreateConnection();
        return await conn.QueryAsync<DealSummaryDto>(sql, new { FromDate = fromDate, ToDate = toDate });
    }

    public async Task<IEnumerable<DealDetailDto>> GetPendingApprovalsDapperAsync(CancellationToken ct = default)
    {
        const string sql = """
            SELECT
                dd.DEAL_ID          AS DealId,
                dd.DEAL_NO          AS DealNo,
                dd.DEAL_VERSIONID   AS DealVersionId,
                dd.DEAL_BATCHID     AS DealBatchId,
                dd.DEAL_TRANTYPE    AS DealTranType,
                dd.DEAL_POSITION    AS DealPosition,
                dd.DEAL_ENTRYDATE   AS DealEntryDate,
                dd.DEAL_AMOUNT      AS DealAmount,
                dd.DEAL_BANKID      AS DealBankId,
                bm.BANK_NAME        AS BankName,
                dd.DEAL_CURRENCY1   AS DealCurrency1,
                dd.DEAL_CURRENCY2   AS DealCurrency2,
                dd.DEAL_SPOTRATE    AS DealSpotRate,
                dd.DEAL_FORPOINTS   AS DealForPoints,
                dd.DEAL_BANKMARGIN  AS DealBankMargin,
                dd.DEAL_BOOKRATE    AS DealBookRate,
                dd.DEAL_MATDATE     AS DealMatDate,
                dd.DEAL_DEALTYPE    AS DealDealType,
                dd.DEAL_BUSINESS    AS DealBusiness,
                dd.DEAL_CATEGORY    AS DealCategory,
                dd.DEAL_APPSTATUS   AS DealAppStatus,
                dd.DEAL_APPREMARKS  AS DealAppRemarks,
                dd.DEAL_SETSTATUS   AS DealSetStatus,
                dd.DEAL_REMARKS     AS DealRemarks,
                dd.DEAL_IRTYPE      AS DealIrType,
                dd.DEAL_STARTDATE   AS DealStartDate,
                dd.DEAL_LOANAMT     AS DealLoanAmt,
                dd.DEAL_MODIFIEDON  AS DealModifiedOn,
                dd.DEAL_MODIFIEDBY  AS DealModifiedBy
            FROM DEALTICKET_DET dd
            LEFT JOIN DEAL_BANKMASTER bm ON dd.DEAL_BANKID = bm.BANK_ID
            WHERE dd.DEAL_APPSTATUS = 'P'
            ORDER BY dd.DEAL_ENTRYDATE ASC
            """;

        await using var conn = CreateConnection();
        return await conn.QueryAsync<DealDetailDto>(sql);
    }

    public async Task<IEnumerable<(long DealId, decimal GainLossAmt, string ResultType)>> GetPnlReportAsync(
        DateTime fromDate, CancellationToken ct = default)
    {
        const string sql = """
            SELECT
                dd.DEAL_ID          AS DealId,
                ds.SET_GAINLOSSAMT  AS GainLossAmt,
                CASE
                    WHEN ds.SET_GAINLOSSAMT > 0 THEN 'Profit'
                    WHEN ds.SET_GAINLOSSAMT < 0 THEN 'Loss'
                    ELSE 'Break-even'
                END AS ResultType
            FROM DEALTICKET_DET dd
            JOIN DEALTICKET_SET ds ON dd.DEAL_ID = ds.SET_DEALID
            WHERE ds.SET_DATE >= @FromDate
            ORDER BY ds.SET_DATE DESC
            """;

        await using var conn = CreateConnection();
        return await conn.QueryAsync<(long, decimal, string)>(sql, new { FromDate = fromDate });
    }
}
