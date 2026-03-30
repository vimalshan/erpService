using System.Data;
using Dapper;
using EmployeeTransactionsService.Application.Contracts;
using EmployeeTransactionsService.Application.DTOs;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Polly;

namespace EmployeeTransactionsService.Infrastructure.Dapper;

public sealed class TransactionReadService(IConfiguration configuration, ResiliencePipeline pipeline) : ITransactionReadService
{
    private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");

    public async Task<IReadOnlyList<TransactionTimelineItemDto>> GetEmployeeTimelineAsync(decimal employeeId, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT 'EmployeeCreated' AS TransactionType,
                   EMP_SYSID AS ReferenceId,
                   ISNULL(EMP_CREATEDON, EMP_APP_DATE) AS ActivityOnUtc,
                   CONCAT('Employee created for ', EMP_FRS_NAME, ' ', ISNULL(EMP_LST_NAME, '')) AS Description
            FROM EMPLOYEE_MAIN
            WHERE EMP_SYSID = @EmployeeId

            UNION ALL

            SELECT 'GradeChanged' AS TransactionType,
                   EMP_GRADECHANGEID AS ReferenceId,
                   EMP_CREATEDON AS ActivityOnUtc,
                   CONCAT('Grade changed from ', EMP_OLDGRADE, ' to ', EMP_NEWGRADE) AS Description
            FROM EMP_GRADECHANGE
            WHERE EMP_EMPSYSID = @EmployeeId

            UNION ALL

            SELECT 'ProbationReviewed' AS TransactionType,
                   PROB_ID AS ReferenceId,
                   ISNULL(PROB_REVIEWDATE, PROB_DUEDATE) AS ActivityOnUtc,
                   CONCAT('Probation status: ', ISNULL(PROB_FINSTATUS, 'Pending')) AS Description
            FROM AA_EMP_PROBATION
            WHERE PROB_EMP_SYSID = @EmployeeId

            ORDER BY ActivityOnUtc DESC;
            """;

        return await pipeline.ExecuteAsync(async token =>
        {
            await using var connection = new SqlConnection(_connectionString);
            var command = new CommandDefinition(sql, new { EmployeeId = employeeId }, cancellationToken: token);
            var rows = await connection.QueryAsync<TransactionTimelineItemDto>(command);
            return rows.ToList();
        }, cancellationToken);
    }
}