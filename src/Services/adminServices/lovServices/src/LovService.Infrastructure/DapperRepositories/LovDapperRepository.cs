using Dapper;
using LovService.Application.DTOs;
using LovService.Application.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace LovService.Infrastructure.DapperRepositories;

/// <summary>
/// Dapper-based read repository for LOV types (fast reads, reporting).
/// </summary>
public class LovTypeDapperRepository(IConfiguration configuration)
{
    private readonly string _connectionString = configuration.GetConnectionString("LovDb")!;

    public async Task<IEnumerable<LovTypeDto>> GetAllAsync(CancellationToken ct = default)
    {
        await using var conn = new SqlConnection(_connectionString);
        var result = await conn.QueryAsync<LovTypeDto>(
            "SELECT LOV_TYPE_ID AS LovTypeId, LOV_TYPE_NAME AS LovTypeName FROM LOV_TYPE WITH (NOLOCK)");
        return result;
    }

    public async Task<LovTypeDto?> GetByIdAsync(long id)
    {
        await using var conn = new SqlConnection(_connectionString);
        return await conn.QueryFirstOrDefaultAsync<LovTypeDto>(
            "SELECT LOV_TYPE_ID AS LovTypeId, LOV_TYPE_NAME AS LovTypeName FROM LOV_TYPE WHERE LOV_TYPE_ID = @Id",
            new { Id = id });
    }

    public async Task<IEnumerable<LovMasterDto>> GetMastersByTypeAsync(long lovTypeId)
    {
        await using var conn = new SqlConnection(_connectionString);
        return await conn.QueryAsync<LovMasterDto>(
            @"SELECT LOV_ID AS LovId, LOV_TYPE_ID AS LovTypeId, LOV_NAME AS LovName,
                     LOV_UPDATED_BY AS LovUpdatedBy, LOV_UPDATED_ON AS LovUpdatedOn
              FROM LOV_MASTER WITH (NOLOCK)
              WHERE LOV_TYPE_ID = @LovTypeId",
            new { LovTypeId = lovTypeId });
    }

    public async Task<IEnumerable<ItemDataDto>> SearchItemDataAsync(string? catName, string? itemName)
    {
        await using var conn = new SqlConnection(_connectionString);
        return await conn.QueryAsync<ItemDataDto>(
            @"SELECT ID AS Id, CATNAME AS CatName, ITEMNAME AS ItemName, MAKE AS Make, UOM AS Uom, PRICE AS Price
              FROM ITEMDATA WITH (NOLOCK)
              WHERE (@CatName IS NULL OR CATNAME LIKE '%' + @CatName + '%')
                AND (@ItemName IS NULL OR ITEMNAME LIKE '%' + @ItemName + '%')",
            new { CatName = catName, ItemName = itemName });
    }

    public async Task<IEnumerable<LovMasterDto>> ExecGetLovMastersByTypeSpAsync(long lovTypeId)
    {
        await using var conn = new SqlConnection(_connectionString);
        return await conn.QueryAsync<LovMasterDto>(
            "usp_GetLovMastersByType",
            new { LovTypeId = lovTypeId },
            commandType: System.Data.CommandType.StoredProcedure);
    }
}
