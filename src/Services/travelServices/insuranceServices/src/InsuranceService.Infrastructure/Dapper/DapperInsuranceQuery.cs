using Dapper;
using InsuranceService.Application.DTOs;
using Microsoft.Data.SqlClient;

namespace InsuranceService.Infrastructure.Dapper;

public interface IDapperInsuranceQuery
{
    Task<IEnumerable<TravelInsuranceDto>> GetInsuranceDetailsAsync(string? companyCode, long? planNumber);
    Task<TravelInsuranceDto?> ExecuteGetByStoredProcAsync(string companyCode, long planNumber);
}

public class DapperInsuranceQuery : IDapperInsuranceQuery
{
    private readonly string _connectionString;

    public DapperInsuranceQuery(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<TravelInsuranceDto>> GetInsuranceDetailsAsync(string? companyCode, long? planNumber)
    {
        await using var connection = new SqlConnection(_connectionString);

        return await connection.QueryAsync<TravelInsuranceDto>(
            "dbo.usp_GetInsuranceDetails",
            new { p_CompanyCode = companyCode, p_PlanNum = planNumber },
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task<TravelInsuranceDto?> ExecuteGetByStoredProcAsync(string companyCode, long planNumber)
    {
        await using var connection = new SqlConnection(_connectionString);

        var results = await connection.QueryAsync<TravelInsuranceDto>(
            "dbo.usp_GetInsuranceDetails",
            new { p_CompanyCode = companyCode, p_PlanNum = planNumber },
            commandType: System.Data.CommandType.StoredProcedure);

        return results.FirstOrDefault();
    }
}
