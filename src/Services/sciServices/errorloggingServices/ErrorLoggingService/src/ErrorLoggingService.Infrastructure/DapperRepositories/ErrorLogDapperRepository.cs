using Dapper;
using ErrorLoggingService.Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ErrorLoggingService.Infrastructure.DapperRepositories;

/// <summary>
/// Read-only Dapper repository that wraps the stored procedures usp_LogError and usp_GetErrorLog.
/// </summary>
public sealed class ErrorLogDapperRepository
{
    private readonly string _connectionString;

    public ErrorLogDapperRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
    }

    public async Task<IEnumerable<dynamic>> GetErrorLogAsync(DateTime startDate, DateTime endDate)
    {
        await using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync(
            "dbo.usp_GetErrorLog",
            new { p_StartDate = startDate, p_EndDate = endDate },
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task LogErrorAsync(string errorMessage, string storedProcedureName, int? errorReference)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.ExecuteAsync(
            "dbo.usp_LogError",
            new
            {
                p_ErrorMessage = errorMessage,
                p_StoredProcedureName = storedProcedureName,
                p_ErrorReference = errorReference
            },
            commandType: System.Data.CommandType.StoredProcedure);
    }
}
