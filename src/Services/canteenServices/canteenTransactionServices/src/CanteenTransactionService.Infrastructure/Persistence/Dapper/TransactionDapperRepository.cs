using Dapper;
using Microsoft.Data.SqlClient;
using CanteenTransactionService.Application.DTOs;

namespace CanteenTransactionService.Infrastructure.Persistence.Dapper;

public class TransactionDapperRepository
{
    private readonly string _connectionString;

    public TransactionDapperRepository(string connectionString) => _connectionString = connectionString;

    public async Task<IEnumerable<TransactionSummaryDto>> GetDailySummaryAsync(long companyCode, string swipeDate)
    {
        const string sql = """
            SELECT
                CN_COM_COD AS CompanyCode,
                CN_SWP_DAT AS SwipeDate,
                COUNT(*) AS TotalTransactions,
                ISNULL(SUM(CN_EE_CON), 0) AS TotalEmployeeContribution,
                ISNULL(SUM(CN_ER_CON), 0) AS TotalEmployerContribution
            FROM CANTEEDN_DACON
            WHERE CN_COM_COD = @CompanyCode
              AND CN_SWP_DAT LIKE @SwipeDate + '%'
            GROUP BY CN_COM_COD, CN_SWP_DAT
            ORDER BY CN_SWP_DAT
            """;

        using var conn = new SqlConnection(_connectionString);
        return await conn.QueryAsync<TransactionSummaryDto>(sql, new { CompanyCode = companyCode, SwipeDate = swipeDate });
    }

    public async Task<IEnumerable<CanteenDaconDto>> GetEmployeeTransactionsAsync(long employeeSysId, string fromDate, string toDate)
    {
        const string sql = """
            SELECT
                CN_SRL_NUM AS SerialNumber,
                CN_COM_COD AS CompanyCode,
                CN_SYS_ID AS EmployeeSysId,
                CN_EMP_TYP AS EmployeeType,
                CN_SWP_DAT AS SwipeDate,
                CN_ITM_COD AS ItemCode,
                CN_ITM_TYP AS ItemType,
                CN_EE_CON AS EmployeeContribution,
                CN_ER_CON AS EmployerContribution,
                CN_CAN_NUM AS CanteenNumber,
                CN_ITM_QTY AS ItemQuantity,
                CN_ENT_USR AS EntryUser,
                CN_ENT_DAT AS EntryDate,
                CN_GRD_CAT AS GradeCategory
            FROM CANTEEDN_DACON
            WHERE CN_SYS_ID = @EmployeeSysId
              AND CN_SWP_DAT >= @FromDate
              AND CN_SWP_DAT <= @ToDate
            ORDER BY CN_SWP_DAT
            """;

        using var conn = new SqlConnection(_connectionString);
        return await conn.QueryAsync<CanteenDaconDto>(sql, new { EmployeeSysId = employeeSysId, FromDate = fromDate, ToDate = toDate });
    }
}
