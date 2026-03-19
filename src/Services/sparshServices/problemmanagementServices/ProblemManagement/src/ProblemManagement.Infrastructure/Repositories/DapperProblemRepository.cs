using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ProblemManagement.Application.DTOs;

namespace ProblemManagement.Infrastructure.Repositories;

public interface IDapperProblemRepository
{
    Task<IEnumerable<ProblemDto>> GetProblemsByStatusAsync(char status);
    Task<IEnumerable<ProblemSolutionDto>> GetSolutionsByProblemIdAsync(long problemId);
    Task<ProblemDto?> GetProblemByIdAsync(long id);
}

public class DapperProblemRepository : IDapperProblemRepository
{
    private readonly string _connectionString;

    public DapperProblemRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not configured.");
    }

    private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    public async Task<IEnumerable<ProblemDto>> GetProblemsByStatusAsync(char status)
    {
        using var connection = CreateConnection();
        const string sql = """
            SELECT PR_ID as PrId, PR_OWNER as PrOwner, PR_ENTEREDBY as PrEnteredBy,
                   PR_DESCRIPTION as PrDescription, PR_RESPEXPBY as PrRespExpBy,
                   PR_CATEGORY as PrCategory, PR_IMPACT as PrImpact,
                   PR_EXPRESULT as PrExpResult, PR_ENTEREDON as PrEnteredOn,
                   PR_STATUS as PrStatus, PR_STATEMENT as PrStatement,
                   PR_TYPE as PrType, PR_UNITID as PrUnitId,
                   PR_SITEID as PrSiteId, PR_MODON as PrModOn
            FROM PROBLEM_MAIN
            WHERE PR_STATUS = @Status
            ORDER BY PR_ENTEREDON DESC
            """;
        return await connection.QueryAsync<ProblemDto>(sql, new { Status = status });
    }

    public async Task<IEnumerable<ProblemSolutionDto>> GetSolutionsByProblemIdAsync(long problemId)
    {
        using var connection = CreateConnection();
        const string sql = """
            SELECT SOL_ID as SolId, SOL_PRID as SolPrId, SOL_DESCRIPTION as SolDescription,
                   SOL_IMPLEMENTATION as SolImplementation, SOL_ENTEREDBY as SolEnteredBy,
                   SOL_ENTEREDON as SolEnteredOn, SOL_ATTACH as SolAttach
            FROM PROBLEM_SOLUTION
            WHERE SOL_PRID = @ProblemId
            ORDER BY SOL_ENTEREDON DESC
            """;
        return await connection.QueryAsync<ProblemSolutionDto>(sql, new { ProblemId = problemId });
    }

    public async Task<ProblemDto?> GetProblemByIdAsync(long id)
    {
        using var connection = CreateConnection();
        const string sql = """
            SELECT PR_ID as PrId, PR_OWNER as PrOwner, PR_ENTEREDBY as PrEnteredBy,
                   PR_DESCRIPTION as PrDescription, PR_RESPEXPBY as PrRespExpBy,
                   PR_CATEGORY as PrCategory, PR_IMPACT as PrImpact,
                   PR_EXPRESULT as PrExpResult, PR_ENTEREDON as PrEnteredOn,
                   PR_STATUS as PrStatus, PR_STATEMENT as PrStatement,
                   PR_TYPE as PrType, PR_UNITID as PrUnitId,
                   PR_SITEID as PrSiteId, PR_MODON as PrModOn
            FROM PROBLEM_MAIN
            WHERE PR_ID = @Id
            """;
        return await connection.QueryFirstOrDefaultAsync<ProblemDto>(sql, new { Id = id });
    }
}
