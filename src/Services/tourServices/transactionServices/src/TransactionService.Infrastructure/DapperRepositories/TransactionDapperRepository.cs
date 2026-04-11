using Dapper;
using Microsoft.Data.SqlClient;
using TransactionService.Application.DTOs;

namespace TransactionService.Infrastructure.DapperRepositories;

public sealed class TransactionDapperRepository
{
    private readonly string _connectionString;

    public TransactionDapperRepository(string connectionString) => _connectionString = connectionString;

    public async Task<IEnumerable<EmployeeJVDto>> GetEmployeeJVSummaryAsync(CancellationToken ct = default)
    {
        const string sql = """
            SELECT
                m.JV_BATCHID       AS JvBatchId,
                m.JV_TPID          AS JvTpId,
                m.JV_TYPE          AS JvType,
                m.JV_DATE          AS JvDate,
                m.JV_EMPSYSID      AS JvEmpSysId,
                m.JV_STATUS        AS JvStatus,
                m.JV_TRNTYPE       AS JvTrnType,
                m.JV_ORAREFNO      AS JvOraRefNo,
                m.JV_NETAMT        AS JvNetAmt,
                m.JV_PAYUNITID     AS JvPayUnitId,
                m.JV_TRNREFNO      AS JvTrnRefNo
            FROM JVEMP_MAIN m
            ORDER BY m.JV_CREATEDON DESC
            """;

        await using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<EmployeeJVDto>(sql);
    }

    public async Task<IEnumerable<TravelBatchDto>> GetTravelBatchSummaryAsync(CancellationToken ct = default)
    {
        const string sql = """
            SELECT
                m.BATCH_ID            AS BatchId,
                m.BATCH_ADMINID       AS AdminId,
                m.BATCH_PAYUNITID     AS PayUnitId,
                m.BATCH_BATCHDATE     AS BatchDate,
                m.BATCH_INVNUM        AS InvNum,
                m.BATCH_INVAMOUNT     AS InvAmount,
                m.BATCH_STATUS        AS Status,
                m.BATCH_VENDORID      AS VendorId,
                m.BATCH_APPAMT        AS ApprovedAmount,
                m.BATCH_TOTPAY        AS TotalPayable,
                m.BATCH_JVID          AS JvId,
                m.BATCH_TYPE          AS BatchType,
                m.BATCH_CREATEDBY     AS CreatedBy,
                m.BATCH_CREATEDON     AS CreatedOn
            FROM TRAVEL_BATCHMAIN m
            ORDER BY m.BATCH_CREATEDON DESC
            """;

        await using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<TravelBatchDto>(sql);
    }
}
