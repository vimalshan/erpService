using Dapper;
using Microsoft.Data.SqlClient;
using WMTransactional.Application.DTOs;

namespace WMTransactional.Infrastructure.Dapper;

public interface IDapperTransactionalQueries
{
    Task<IEnumerable<PurchaseOrderDto>> GetPurchaseOrdersBySupplierAsync(int supplierId);
    Task<IEnumerable<SalesOrderDto>> GetSalesOrdersByCustomerAsync(int customerId);
    Task<IEnumerable<ShipmentDto>> GetShipmentsBySalesOrderAsync(int soId);
    Task<IEnumerable<ReceivingDto>> GetReceivingsByPurchaseOrderAsync(int poId);
}

public class DapperTransactionalQueries : IDapperTransactionalQueries
{
    private readonly string _connectionString;

    public DapperTransactionalQueries(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<PurchaseOrderDto>> GetPurchaseOrdersBySupplierAsync(int supplierId)
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<PurchaseOrderDto>(
            @"SELECT po_id AS PoId, po_number AS PoNumber, supplier_id AS SupplierId, 
                     order_date AS OrderDate, expected_date AS ExpectedDate, status AS Status, 
                     notes AS Notes, created_by AS CreatedBy, created_date AS CreatedDate, modified_date AS ModifiedDate
              FROM PurchaseOrder WHERE supplier_id = @SupplierId ORDER BY created_date DESC",
            new { SupplierId = supplierId });
    }

    public async Task<IEnumerable<SalesOrderDto>> GetSalesOrdersByCustomerAsync(int customerId)
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<SalesOrderDto>(
            @"SELECT so_id AS SoId, so_number AS SoNumber, customer_id AS CustomerId, 
                     order_date AS OrderDate, requested_date AS RequestedDate, status AS Status, 
                     notes AS Notes, created_by AS CreatedBy, created_date AS CreatedDate, modified_date AS ModifiedDate
              FROM SalesOrder WHERE customer_id = @CustomerId ORDER BY created_date DESC",
            new { CustomerId = customerId });
    }

    public async Task<IEnumerable<ShipmentDto>> GetShipmentsBySalesOrderAsync(int soId)
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<ShipmentDto>(
            @"SELECT shipment_id AS ShipmentId, shipment_number AS ShipmentNumber, so_id AS SoId,
                     shipped_date AS ShippedDate, status AS Status, tracking_number AS TrackingNumber,
                     carrier AS Carrier, notes AS Notes, created_by AS CreatedBy, created_date AS CreatedDate
              FROM Shipment WHERE so_id = @SoId ORDER BY created_date DESC",
            new { SoId = soId });
    }

    public async Task<IEnumerable<ReceivingDto>> GetReceivingsByPurchaseOrderAsync(int poId)
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<ReceivingDto>(
            @"SELECT receiving_id AS ReceivingId, receiving_number AS ReceivingNumber, po_id AS PoId,
                     received_date AS ReceivedDate, status AS Status, notes AS Notes,
                     created_by AS CreatedBy, created_date AS CreatedDate
              FROM Receiving WHERE po_id = @PoId ORDER BY created_date DESC",
            new { PoId = poId });
    }
}
