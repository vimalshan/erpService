using Dapper;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DocumentService.Functions.Functions;

/// <summary>
/// Runs daily at 08:00 UTC to log documents that have not been modified in over 30 days,
/// flagging them for manual review or automated follow-up.
/// </summary>
public class DocumentExpiryCheckerFunction
{
    private readonly ILogger<DocumentExpiryCheckerFunction> _logger;
    private readonly string _connectionString;

    public DocumentExpiryCheckerFunction(ILogger<DocumentExpiryCheckerFunction> logger, IConfiguration configuration)
    {
        _logger = logger;
        _connectionString = configuration["SqlConnectionString"]
            ?? throw new InvalidOperationException("SqlConnectionString is not configured.");
    }

    [Function("DocumentExpiryChecker")]
    public async Task Run(
        [TimerTrigger("%DocumentExpiryCheckSchedule%")] TimerInfo timer,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("DocumentExpiryChecker triggered at {Time}", DateTimeOffset.UtcNow);

        try
        {
            using var connection = new SqlConnection(_connectionString);
            const string sql = """
                SELECT LOANDOC_ID, LOANDOC_LOANID, LOANDOC_TYPEID, LOANDOC_LASTMODIFIEDON
                FROM LOAN_DOCUMENTS
                WHERE LOANDOC_LASTMODIFIEDON < DATEADD(DAY, -30, GETUTCDATE())
                ORDER BY LOANDOC_LASTMODIFIEDON ASC
                """;

            var staleDocuments = await connection.QueryAsync<dynamic>(
                new CommandDefinition(sql, cancellationToken: cancellationToken));

            var count = 0;
            foreach (var doc in staleDocuments)
            {
                var docId = (long)doc.LOANDOC_ID;
                var loanId = (long)doc.LOANDOC_LOANID;
                var typeId = (long)doc.LOANDOC_TYPEID;
                var lastModified = (DateTime)doc.LOANDOC_LASTMODIFIEDON;
                _logger.LogWarning(
                    "Stale document detected — ID: {Id}, LoanID: {LoanId}, TypeID: {TypeId}, LastModified: {LastModified}",
                    docId, loanId, typeId, lastModified);
                count++;
            }

            _logger.LogInformation("DocumentExpiryChecker: found {Count} stale document(s).", count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "DocumentExpiryChecker failed.");
            throw;
        }
    }
}
