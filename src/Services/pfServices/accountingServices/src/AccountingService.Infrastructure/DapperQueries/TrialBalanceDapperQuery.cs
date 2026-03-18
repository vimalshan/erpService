using AccountingService.Application.DTOs;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace AccountingService.Infrastructure.DapperQueries;

public class TrialBalanceDapperQuery
{
    private readonly string _connectionString;

    public TrialBalanceDapperQuery(IConfiguration configuration)
        => _connectionString = configuration.GetConnectionString("DefaultConnection")!;

    public async Task<IEnumerable<TrialBalanceDto>> GetTrialBalanceAsync(CancellationToken ct = default)
    {
        const string sql = """
            SELECT 
                gp.ACCOUNT_CODE       AS AccountCode,
                ma.MAIN_ACCOUNT_NAME  AS AccountName,
                SUM(ISNULL(gp.DEBIT_AMOUNT,  0)) AS TotalDebit,
                SUM(ISNULL(gp.CREDIT_AMOUNT, 0)) AS TotalCredit,
                SUM(ISNULL(gp.DEBIT_AMOUNT,  0)) - SUM(ISNULL(gp.CREDIT_AMOUNT, 0)) AS Balance
            FROM dbo.GL_POSTING gp
            LEFT JOIN dbo.MAINACCOUNT_MASTER ma ON gp.ACCOUNT_CODE = ma.MAIN_ACCOUNT_CODE
            GROUP BY gp.ACCOUNT_CODE, ma.MAIN_ACCOUNT_NAME
            ORDER BY gp.ACCOUNT_CODE
            """;

        await using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<TrialBalanceDto>(
            new CommandDefinition(sql, cancellationToken: ct));
    }

    public async Task<IEnumerable<TransactionJournalDto>> GetTransactionJournalAsync(
        string? trustCode = null, CancellationToken ct = default)
    {
        const string sql = """
            SELECT 
                t.TRANSACTION_ID        AS TransactionId,
                t.TD_TRANSACTION_CODE   AS TransactionCode,
                tm.TRANSACTION_NAME     AS TransactionName,
                t.TD_TRANSACTION_DATE   AS TransactionDate,
                t.TD_MEMBER_NO          AS MemberNo,
                t.TD_AMOUNT             AS Amount,
                t.TD_TYPE_CODE          AS TypeCode,
                t.TD_REMARKS            AS Remarks
            FROM dbo.TRAN_DET t
            LEFT JOIN dbo.TRANSACTION_MASTER tm 
                ON t.TD_TRANSACTION_CODE = tm.TRANSACTION_CODE
            WHERE t.TD_CANCEL_STATUS IS NULL
              AND (@TrustCode IS NULL OR t.TD_TRUST_CODE = @TrustCode)
            ORDER BY t.TD_TRANSACTION_DATE DESC
            """;

        await using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<TransactionJournalDto>(
            new CommandDefinition(sql, new { TrustCode = trustCode }, cancellationToken: ct));
    }

    public async Task PostGlEntryViaProcedure(
        string accountCode, decimal debitAmount, decimal creditAmount,
        long referenceId, DateTime postingDate, string remarks, long postedBy,
        CancellationToken ct = default)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.ExecuteAsync(
            new CommandDefinition(
                "dbo.usp_PostGLEntry",
                new
                {
                    p_AccountCode = accountCode,
                    p_DebitAmount = debitAmount,
                    p_CreditAmount = creditAmount,
                    p_ReferenceID = referenceId,
                    p_PostingDate = postingDate,
                    p_Remarks = remarks,
                    p_PostedBy = postedBy
                },
                commandType: System.Data.CommandType.StoredProcedure,
                cancellationToken: ct));
    }
}

public record TransactionJournalDto(
    int TransactionId,
    string TransactionCode,
    string? TransactionName,
    DateTime TransactionDate,
    int? MemberNo,
    decimal Amount,
    string TypeCode,
    string? Remarks
);
