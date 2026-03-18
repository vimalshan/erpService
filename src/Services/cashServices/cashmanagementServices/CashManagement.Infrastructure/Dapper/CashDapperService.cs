using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using CashManagement.Application.DTOs;

namespace CashManagement.Infrastructure.Dapper;

public class CashDapperService
{
    private readonly string _connectionString;

    public CashDapperService(IConfiguration configuration)
        => _connectionString = configuration.GetConnectionString("DefaultConnection")!;

    public async Task<IEnumerable<CashTransactionDto>> GetCashTransactionsByDateRangeAsync(
        long cashUnitId, DateTime from, DateTime to)
    {
        using var conn = new SqlConnection(_connectionString);
        const string sql = """
            SELECT 
                CASH_TXN_ID as CashTxnId,
                CASH_UNIT_ID as CashUnitId,
                CASH_TXN_TYPE as TxnType,
                CASH_TXN_AMOUNT as Amount,
                CASH_TXN_SOURCE as Source,
                CASH_TXN_PAYEE_ID as PayeeId,
                CASH_TXN_REF_NO as RefNo,
                CASH_TXN_DATE as TxnDate,
                CASH_TXN_REMARKS as Remarks,
                CASH_TXN_STATUS as Status,
                AUTHORIZED_BY as AuthorizedBy,
                CREATED_BY as CreatedBy,
                CREATED_ON as CreatedOn
            FROM CASH_TRANSACTION
            WHERE CASH_UNIT_ID = @CashUnitId
              AND CASH_TXN_DATE BETWEEN @From AND @To
            ORDER BY CASH_TXN_DATE DESC
            """;
        return await conn.QueryAsync<CashTransactionDto>(sql, new { CashUnitId = cashUnitId, From = from, To = to });
    }

    public async Task<decimal> GetCashInHandFastAsync(long cashUnitId, DateTime asOfDate)
    {
        using var conn = new SqlConnection(_connectionString);
        return await conn.ExecuteScalarAsync<decimal>(
            "SELECT dbo.fn_GetCashInHand(@CashUnitId, @AsOfDate)",
            new { CashUnitId = cashUnitId, AsOfDate = asOfDate });
    }

    public async Task<decimal> GetBankBalanceFastAsync(long bankAccountId, DateTime asOfDate)
    {
        using var conn = new SqlConnection(_connectionString);
        return await conn.ExecuteScalarAsync<decimal>(
            "SELECT dbo.fn_GetBankBalance(@BankAccountId, @AsOfDate)",
            new { BankAccountId = bankAccountId, AsOfDate = asOfDate });
    }

    public async Task<decimal> GetUnclearedChequesFastAsync(long bankAccountId, DateTime asOfDate)
    {
        using var conn = new SqlConnection(_connectionString);
        return await conn.ExecuteScalarAsync<decimal>(
            "SELECT dbo.fn_GetUnclearedChequesTotal(@BankAccountId, @AsOfDate)",
            new { BankAccountId = bankAccountId, AsOfDate = asOfDate });
    }

    public async Task<IEnumerable<ChequeDto>> GetChequesByStatusAsync(long bankAccountId, string status)
    {
        using var conn = new SqlConnection(_connectionString);
        const string sql = """
            SELECT 
                CHEQUE_ID as ChequeId,
                BANK_ACCOUNT_ID as BankAccountId,
                CHEQUE_NUMBER as ChequeNumber,
                PAYEE_NAME as PayeeName,
                CHEQUE_AMOUNT as ChequeAmount,
                CHEQUE_ISSUE_DATE as IssueDate,
                CHEQUE_DATE as ChequeDate,
                CHEQUE_REFERENCE as Reference,
                CHEQUE_STATUS as Status,
                CHEQUE_BOUNCE_REASON as BounceReason,
                CREATED_ON as CreatedOn
            FROM CHEQUE_REGISTER
            WHERE BANK_ACCOUNT_ID = @BankAccountId AND CHEQUE_STATUS = @Status
            ORDER BY CHEQUE_ISSUE_DATE DESC
            """;
        return await conn.QueryAsync<ChequeDto>(sql, new { BankAccountId = bankAccountId, Status = status });
    }

    public async Task<int> GetOverdueIssuedChequesCountAsync(DateTime asOfDate)
    {
        using var conn = new SqlConnection(_connectionString);
        const string sql = """
            SELECT COUNT(*) FROM CHEQUE_REGISTER
            WHERE CHEQUE_STATUS = 'I' AND CHEQUE_DATE < @AsOfDate
            """;
        return await conn.ExecuteScalarAsync<int>(sql, new { AsOfDate = asOfDate });
    }

    public async Task<IEnumerable<long>> GetAccountsPendingReconciliationAsync(int year, int month)
    {
        using var conn = new SqlConnection(_connectionString);
        const string sql = """
            SELECT ba.BANK_ACCOUNT_ID
            FROM BANK_ACCOUNT ba
            WHERE ba.BANK_ACCOUNT_STATUS = 'A'
              AND NOT EXISTS (
                  SELECT 1 FROM BANK_RECONCILIATION br
                  WHERE br.BANK_ACCOUNT_ID = ba.BANK_ACCOUNT_ID
                    AND YEAR(br.RECONCILIATION_DATE) = @Year
                    AND MONTH(br.RECONCILIATION_DATE) = @Month
              )
            """;
        return await conn.QueryAsync<long>(sql, new { Year = year, Month = month });
    }
}

