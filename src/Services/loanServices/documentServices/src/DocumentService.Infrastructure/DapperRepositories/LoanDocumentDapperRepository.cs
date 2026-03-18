using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using DocumentService.Domain.Entities;

namespace DocumentService.Infrastructure.DapperRepositories;

/// <summary>
/// Dapper-based read repository for high-performance query scenarios.
/// </summary>
public sealed class LoanDocumentDapperRepository
{
    private readonly string _connectionString;

    public LoanDocumentDapperRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    public async Task<LoanDocumentRecord?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        using var connection = new SqlConnection(_connectionString);
        const string sql = """
            SELECT LOANDOC_ID AS Id,
                   LOANDOC_LOANID AS LoanId,
                   LOANDOC_TYPEID AS TypeId,
                   LOANDOC_LASTMODIFIEDBY AS LastModifiedBy,
                   LOANDOC_LASTMODIFIEDON AS LastModifiedOn
            FROM LOAN_DOCUMENTS
            WHERE LOANDOC_ID = @Id
            """;
        var command = new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken);
        return await connection.QueryFirstOrDefaultAsync<LoanDocumentRecord>(command);
    }

    public async Task<IEnumerable<LoanDocumentRecord>> GetByLoanIdAsync(long loanId, CancellationToken cancellationToken = default)
    {
        using var connection = new SqlConnection(_connectionString);
        const string sql = """
            SELECT LOANDOC_ID AS Id,
                   LOANDOC_LOANID AS LoanId,
                   LOANDOC_TYPEID AS TypeId,
                   LOANDOC_LASTMODIFIEDBY AS LastModifiedBy,
                   LOANDOC_LASTMODIFIEDON AS LastModifiedOn
            FROM LOAN_DOCUMENTS
            WHERE LOANDOC_LOANID = @LoanId
            ORDER BY LOANDOC_LASTMODIFIEDON DESC
            """;
        var command = new CommandDefinition(sql, new { LoanId = loanId }, cancellationToken: cancellationToken);
        return await connection.QueryAsync<LoanDocumentRecord>(command);
    }

    public async Task<IEnumerable<LoanDocumentRecord>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        using var connection = new SqlConnection(_connectionString);
        const string sql = """
            SELECT LOANDOC_ID AS Id,
                   LOANDOC_LOANID AS LoanId,
                   LOANDOC_TYPEID AS TypeId,
                   LOANDOC_LASTMODIFIEDBY AS LastModifiedBy,
                   LOANDOC_LASTMODIFIEDON AS LastModifiedOn
            FROM LOAN_DOCUMENTS
            ORDER BY LOANDOC_LASTMODIFIEDON DESC
            """;
        var command = new CommandDefinition(sql, cancellationToken: cancellationToken);
        return await connection.QueryAsync<LoanDocumentRecord>(command);
    }

    public record LoanDocumentRecord(long Id, long LoanId, long TypeId, long LastModifiedBy, DateTime LastModifiedOn);
}
