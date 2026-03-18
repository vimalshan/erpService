using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using UtilityService.Application.DTOs;

namespace UtilityService.Infrastructure.Repositories;

public class ToadPlanSqlDapperRepository
{
    private readonly string _connectionString;

    public ToadPlanSqlDapperRepository(IConfiguration configuration)
        => _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

    private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    public async Task<IEnumerable<ToadPlanSqlDto>> GetByUsernameRawAsync(string username)
    {
        const string sql = """
            SELECT ID as Id, USERNAME as Username, STATEMENT_ID as StatementId,
                   TIMESTAMP as Timestamp, STATEMENT as Statement,
                   CREATED_AT as CreatedAt, UPDATED_AT as UpdatedAt
            FROM TOAD_PLAN_SQL
            WHERE USERNAME = @Username AND IS_DELETED = 0
            ORDER BY CREATED_AT DESC
            """;

        using var connection = CreateConnection();
        return await connection.QueryAsync<ToadPlanSqlDto>(sql, new { Username = username });
    }

    public async Task<ToadPlanSqlDto?> GetByStatementIdRawAsync(string statementId)
    {
        const string sql = """
            SELECT ID as Id, USERNAME as Username, STATEMENT_ID as StatementId,
                   TIMESTAMP as Timestamp, STATEMENT as Statement,
                   CREATED_AT as CreatedAt, UPDATED_AT as UpdatedAt
            FROM TOAD_PLAN_SQL
            WHERE STATEMENT_ID = @StatementId AND IS_DELETED = 0
            """;

        using var connection = CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<ToadPlanSqlDto>(sql, new { StatementId = statementId });
    }

    public async Task<IEnumerable<ToadPlanSqlDto>> SearchAsync(string? username, string? statementFragment)
    {
        const string sql = """
            SELECT ID as Id, USERNAME as Username, STATEMENT_ID as StatementId,
                   TIMESTAMP as Timestamp, STATEMENT as Statement,
                   CREATED_AT as CreatedAt, UPDATED_AT as UpdatedAt
            FROM TOAD_PLAN_SQL
            WHERE IS_DELETED = 0
              AND (@Username IS NULL OR USERNAME = @Username)
              AND (@StatementFragment IS NULL OR STATEMENT LIKE '%' + @StatementFragment + '%')
            ORDER BY CREATED_AT DESC
            """;

        using var connection = CreateConnection();
        return await connection.QueryAsync<ToadPlanSqlDto>(sql,
            new { Username = username, StatementFragment = statementFragment });
    }

    public async Task<int> GetStatisticsCountByUserAsync()
    {
        const string sql = """
            SELECT COUNT(*) as PlanCount FROM TOAD_PLAN_SQL WHERE IS_DELETED = 0
            """;

        using var connection = CreateConnection();
        return await connection.ExecuteScalarAsync<int>(sql);
    }
}
