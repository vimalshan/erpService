using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ShipmentService.Application.DTOs;

namespace ShipmentService.Infrastructure.Repositories;

/// <summary>Dapper-based read-side repository for performance-critical queries and stored procedure calls.</summary>
public sealed class DapperShipmentRepository
{
    private readonly string _connectionString;

    public DapperShipmentRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("ShipmentDb")
            ?? throw new InvalidOperationException("ShipmentDb connection string is not configured.");
    }

    private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    public async Task<IEnumerable<ShipmentSummaryDto>> GetShipmentsByStatusAsync(string status)
    {
        const string sql = """
            SELECT shipment_id AS ShipmentId, shipment_number AS ShipmentNumber,
                   customer_id AS CustomerId, warehouse_id AS WarehouseId,
                   shipment_type AS ShipmentType, service_type AS ServiceType,
                   status AS Status, tracking_number AS TrackingNumber,
                   carrier AS Carrier, shipped_date AS ShippedDate, created_date AS CreatedDate
            FROM Shipment WHERE status = @Status
            ORDER BY created_date DESC
            """;

        using var conn = CreateConnection();
        return await conn.QueryAsync<ShipmentSummaryDto>(sql, new { Status = status });
    }

    public async Task<int> CreateShipmentViaSpAsync(
        string shipmentNumber, int customerId, int warehouseId, string shipmentType,
        string? serviceType, string? carrier, string? trackingNumber,
        string? specialInstructions, string? createdBy)
    {
        using var conn = CreateConnection();
        var parameters = new DynamicParameters();
        parameters.Add("@shipment_number", shipmentNumber);
        parameters.Add("@customer_id", customerId);
        parameters.Add("@warehouse_id", warehouseId);
        parameters.Add("@shipment_type", shipmentType);
        parameters.Add("@service_type", serviceType);
        parameters.Add("@carrier", carrier);
        parameters.Add("@tracking_number", trackingNumber);
        parameters.Add("@special_instructions", specialInstructions);
        parameters.Add("@created_by", createdBy);
        parameters.Add("@shipment_id", dbType: DbType.Int32, direction: ParameterDirection.Output);

        await conn.ExecuteAsync("sp_CreateShipment", parameters, commandType: CommandType.StoredProcedure);
        return parameters.Get<int>("@shipment_id");
    }

    public async Task UpdateShipmentStatusViaSpAsync(int shipmentId, string newStatus,
        string? location, string? description, string? updatedBy)
    {
        using var conn = CreateConnection();
        var parameters = new DynamicParameters();
        parameters.Add("@shipment_id", shipmentId);
        parameters.Add("@new_status", newStatus);
        parameters.Add("@location", location);
        parameters.Add("@description", description);
        parameters.Add("@updated_by", updatedBy);

        await conn.ExecuteAsync("sp_UpdateShipmentStatus", parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task<decimal> CalculateShippingCostAsync(decimal weight, string serviceType)
    {
        const string sql = "SELECT dbo.fn_CalculateShippingCost(@weight, @serviceType)";
        using var conn = CreateConnection();
        return await conn.ExecuteScalarAsync<decimal>(sql, new { weight, serviceType });
    }
}
