using Dapper;
using Microsoft.Data.SqlClient;
using InventoryService.Application.DTOs;

namespace InventoryService.Infrastructure.Dapper;

public interface IDapperInventoryQueries
{
    Task<IEnumerable<StockLevelDto>> GetInventoryByWarehouseAsync(int warehouseId);
    Task<decimal> GetAvailableStockAsync(int productId, int? warehouseId = null, int? binId = null);
    Task TransferInventoryAsync(int productId, int fromWarehouseId, int? fromBinId, int toWarehouseId, int? toBinId, decimal quantity, string? referenceNumber, string? createdBy);
    Task AdjustInventoryAsync(int productId, int warehouseId, int binId, decimal newQuantity, string reason, string adjustedBy);
}

public class DapperInventoryQueries : IDapperInventoryQueries
{
    private readonly string _connectionString;

    public DapperInventoryQueries(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<StockLevelDto>> GetInventoryByWarehouseAsync(int warehouseId)
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<StockLevelDto>(
            "sp_GetInventoryByWarehouse",
            new { warehouse_id = warehouseId },
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task<decimal> GetAvailableStockAsync(int productId, int? warehouseId = null, int? binId = null)
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.ExecuteScalarAsync<decimal>(
            "SELECT dbo.fn_GetAvailableStock(@ProductId, @WarehouseId, @BinId)",
            new { ProductId = productId, WarehouseId = warehouseId, BinId = binId });
    }

    public async Task TransferInventoryAsync(int productId, int fromWarehouseId, int? fromBinId,
        int toWarehouseId, int? toBinId, decimal quantity, string? referenceNumber, string? createdBy)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.ExecuteAsync(
            "sp_TransferInventory",
            new
            {
                product_id = productId,
                from_warehouse_id = fromWarehouseId,
                from_bin_id = fromBinId,
                to_warehouse_id = toWarehouseId,
                to_bin_id = toBinId,
                quantity,
                reference_number = referenceNumber,
                created_by = createdBy
            },
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task AdjustInventoryAsync(int productId, int warehouseId, int binId,
        decimal newQuantity, string reason, string adjustedBy)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.ExecuteAsync(
            "sp_AdjustInventory",
            new
            {
                product_id = productId,
                warehouse_id = warehouseId,
                bin_id = binId,
                new_quantity = newQuantity,
                reason,
                adjusted_by = adjustedBy
            },
            commandType: System.Data.CommandType.StoredProcedure);
    }
}
