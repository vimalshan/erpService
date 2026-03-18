using LoanService.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace LoanService.AzureFunctions;

/// <summary>
/// Timer-triggered function to generate monthly loan statements.
/// Runs on the 1st of every month at 6 AM.
/// </summary>
public class MonthlyStatementGenerator
{
    private readonly ILoanDapperRepository _dapper;
    private readonly IBlobStorageService _blobStorage;
    private readonly ILogger<MonthlyStatementGenerator> _logger;

    public MonthlyStatementGenerator(ILoanDapperRepository dapper, IBlobStorageService blobStorage, ILogger<MonthlyStatementGenerator> logger)
    {
        _dapper = dapper;
        _blobStorage = blobStorage;
        _logger = logger;
    }

    // [Function("MonthlyStatementGenerator")]
    // [TimerTrigger("0 0 6 1 * *")]
    public async Task RunAsync(CancellationToken ct)
    {
        _logger.LogInformation("Generating monthly loan statements at {Time}", DateTime.UtcNow);

        const string sql = """
            SELECT LOAN_NO AS LoanNo, LOAN_MEMBER_ID AS MemberId, LOAN_AMOUNT AS LoanAmount, 
                   LOAN_PRINCIPALOS AS PrincipalOutstanding
            FROM LOAN_MAIN
            WHERE LOAN_STATUS = 'A'
            """;

        var activeLoans = await _dapper.QueryAsync<ActiveLoanInfo>(sql, ct: ct);

        foreach (var loan in activeLoans)
        {
            var statement = $"Loan Statement - Loan No: {loan.LoanNo}, Member: {loan.MemberId}, " +
                            $"Amount: {loan.LoanAmount}, Outstanding: {loan.PrincipalOutstanding}, " +
                            $"Generated: {DateTime.UtcNow:yyyy-MM-dd}";

            using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(statement));
            var blobName = $"statements/{loan.LoanNo}/{DateTime.UtcNow:yyyy-MM}.txt";

            await _blobStorage.UploadAsync("loan-documents", blobName, stream, "text/plain", ct);
            _logger.LogInformation("Generated statement for Loan {LoanNo}", loan.LoanNo);
        }
    }
}

public record ActiveLoanInfo(long LoanNo, long MemberId, decimal LoanAmount, decimal PrincipalOutstanding);
