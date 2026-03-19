using System.Data;
using Dapper;
using MamAllocationService.Application.DTOs;
using MamAllocationService.Application.Handlers;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace MamAllocationService.Infrastructure.Dapper;

public class AllocationSummaryDapperQuery(IConfiguration configuration) : IAllocationSummaryDapperQuery
{
    public async Task<AllocationSummaryDto?> ExecuteAsync(DateTime allocationDate, int rawMaterialCode, CancellationToken ct = default)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        await using var connection = new SqlConnection(connectionString);

        var result = await connection.QueryFirstOrDefaultAsync<AllocationSummaryDto>(
            "dbo.usp_GetAllocationSummary",
            new { p_AllocationDate = allocationDate, p_RawMaterialCode = rawMaterialCode },
            commandType: CommandType.StoredProcedure);

        return result;
    }
}
