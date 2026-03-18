using Dapper;
using InventoryManagement.Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace InventoryManagement.Infrastructure.Dapper;

/// <summary>
/// Dapper-based read repository for high-performance item queries.
/// </summary>
public sealed class DapperItemReadRepository
{
    private readonly string _connectionString;

    public DapperItemReadRepository(IConfiguration config)
        => _connectionString = config.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection string not configured.");

    public async Task<IEnumerable<ItemMaster>> GetAllItemsAsync()
    {
        const string sql = @"
            SELECT im.SCI_ITEM_ID AS SciItemId, im.ORACLE_CODE AS OracleCode, im.ORACLE_ITEM_ID AS OracleItemId,
                   im.ITEM_NAME AS ItemName, im.ITEM_TYPE AS ItemType, im.ITEM_UOM_ID AS ItemUomId,
                   im.MAIN_PRODUCT_ID AS MainProductId, im.ISBULK_ITEM AS IsBulkItem,
                   im.ISBULK_SOURCE AS IsBulkSource, im.LEAD_TIME AS LeadTime
            FROM ITEM_MASTER im WITH (NOLOCK)
            ORDER BY im.SCI_ITEM_ID";

        using var conn = new SqlConnection(_connectionString);
        return await conn.QueryAsync<ItemMaster>(sql);
    }

    public async Task<ItemMaster?> GetItemByOracleCodeAsync(string oracleCode)
    {
        const string sql = @"
            SELECT im.SCI_ITEM_ID AS SciItemId, im.ORACLE_CODE AS OracleCode, im.ORACLE_ITEM_ID AS OracleItemId,
                   im.ITEM_NAME AS ItemName, im.ITEM_TYPE AS ItemType, im.ITEM_UOM_ID AS ItemUomId,
                   im.MAIN_PRODUCT_ID AS MainProductId, im.ISBULK_ITEM AS IsBulkItem,
                   im.ISBULK_SOURCE AS IsBulkSource, im.LEAD_TIME AS LeadTime
            FROM ITEM_MASTER im WITH (NOLOCK)
            WHERE im.ORACLE_CODE = @OracleCode";

        using var conn = new SqlConnection(_connectionString);
        return await conn.QueryFirstOrDefaultAsync<ItemMaster>(sql, new { OracleCode = oracleCode });
    }

    public async Task<int> RegisterItemViaStoredProcAsync(
        string oracleCode, string itemName, int? mainProductId, string itemType,
        int unitId, decimal convFactor)
    {
        using var conn = new SqlConnection(_connectionString);
        var parameters = new DynamicParameters();
        parameters.Add("@p_OracleCode", oracleCode);
        parameters.Add("@p_ItemName", itemName);
        parameters.Add("@p_MainProductID", mainProductId);
        parameters.Add("@p_ItemType", itemType);
        parameters.Add("@p_UnitID", unitId);
        parameters.Add("@p_ConversionFactor", convFactor);
        parameters.Add("@p_HierarchyLevel", 1);
        parameters.Add("@p_ItemID", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);

        await conn.ExecuteAsync("dbo.usp_RegisterItem",
            parameters, commandType: System.Data.CommandType.StoredProcedure);

        return parameters.Get<int>("@p_ItemID");
    }
}
