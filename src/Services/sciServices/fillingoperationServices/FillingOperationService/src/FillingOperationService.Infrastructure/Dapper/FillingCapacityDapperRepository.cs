using Dapper;
using FillingOperationService.Application.DTOs;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace FillingOperationService.Infrastructure.Dapper;

public class FillingCapacityDapperRepository(IConfiguration configuration)
{
    private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("DefaultConnection string not configured.");

    public async Task<IEnumerable<FillingCapacityDto>> GetCapacityByGroupAndProductAsync(int groupId, int productId)
    {
        const string sql = """
            EXEC dbo.usp_GetFillingPointCapacity 
                @p_FillingPointGroupID = @GroupId, 
                @p_ProductID = @ProductId
            """;

        await using var connection = new SqlConnection(_connectionString);
        var results = await connection.QueryAsync<FillingCapacityDto>(sql, new { GroupId = groupId, ProductId = productId });
        return results;
    }

    public async Task<IEnumerable<FillingCapacityDto>> GetCapacityByGroupAsync(int groupId)
    {
        const string sql = """
            SELECT FILLING_POINT_GROUP_ID AS FillingPointGroupId,
                   MAIN_PRODUCT_ID        AS MainProductId,
                   PACKAGE_TYPE_ID        AS PackageTypeId,
                   ITEM_CAPACITY_ID       AS ItemCapacityId,
                   CAPACITY_PER_SHIFT     AS CapacityPerShift,
                   USAGE_PRIORITY         AS UsagePriority
            FROM   dbo.FILLING_CAPACITY
            WHERE  FILLING_POINT_GROUP_ID = @GroupId
            ORDER  BY USAGE_PRIORITY
            """;

        await using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<FillingCapacityDto>(sql, new { GroupId = groupId });
    }
}
