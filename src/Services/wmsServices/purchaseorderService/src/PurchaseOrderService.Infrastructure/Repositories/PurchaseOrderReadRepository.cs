using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PurchaseOrderService.Application.DTOs;
using PurchaseOrderService.Application.Interfaces;

namespace PurchaseOrderService.Infrastructure.Repositories;

public class PurchaseOrderReadRepository : IPurchaseOrderReadRepository
{
    private readonly string _connectionString;

    public PurchaseOrderReadRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    public async Task<PurchaseOrderDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            SELECT po.po_id AS PoId, po.po_number AS PoNumber, po.supplier_id AS SupplierId,
                   po.warehouse_id AS WarehouseId, po.order_date AS OrderDate, po.expected_date AS ExpectedDate,
                   po.status AS Status, po.notes AS Notes, po.created_by AS CreatedBy,
                   po.created_date AS CreatedDate, po.modified_date AS ModifiedDate
            FROM PurchaseOrder po
            WHERE po.po_id = @Id;

            SELECT pol.po_line_id AS PoLineId, pol.po_id AS PoId, pol.product_id AS ProductId,
                   pol.line_number AS LineNumber, pol.quantity_ordered AS QuantityOrdered,
                   pol.quantity_received AS QuantityReceived, pol.unit_price AS UnitPrice, pol.notes AS Notes
            FROM PurchaseOrderLine pol
            WHERE pol.po_id = @Id
            ORDER BY pol.line_number;";

        using var connection = new SqlConnection(_connectionString);
        using var multi = await connection.QueryMultipleAsync(sql, new { Id = id });

        var po = await multi.ReadSingleOrDefaultAsync<PurchaseOrderDto>();
        if (po == null) return null;

        var lines = (await multi.ReadAsync<PurchaseOrderLineDto>()).ToList();
        foreach (var line in lines)
        {
            line.LineTotal = line.UnitPrice.HasValue ? line.UnitPrice.Value * line.QuantityOrdered : null;
            line.IsFullyReceived = line.QuantityReceived >= line.QuantityOrdered;
        }
        po.Lines = lines;
        po.TotalAmount = lines.Where(l => l.LineTotal.HasValue).Sum(l => l.LineTotal);

        return po;
    }

    public async Task<PurchaseOrderDto?> GetByPoNumberAsync(string poNumber, CancellationToken cancellationToken = default)
    {
        const string sql = "SELECT po_id FROM PurchaseOrder WHERE po_number = @PoNumber";

        using var connection = new SqlConnection(_connectionString);
        var id = await connection.QuerySingleOrDefaultAsync<int?>(sql, new { PoNumber = poNumber });

        return id.HasValue ? await GetByIdAsync(id.Value, cancellationToken) : null;
    }

    public async Task<IEnumerable<PurchaseOrderSummaryDto>> GetAllAsync(int page, int pageSize, string? status = null, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            SELECT po.po_id AS PoId, po.po_number AS PoNumber, po.supplier_id AS SupplierId,
                   po.warehouse_id AS WarehouseId, po.order_date AS OrderDate, po.status AS Status,
                   (SELECT SUM(pol.unit_price * pol.quantity_ordered) FROM PurchaseOrderLine pol WHERE pol.po_id = po.po_id) AS TotalAmount,
                   (SELECT COUNT(*) FROM PurchaseOrderLine pol WHERE pol.po_id = po.po_id) AS LineCount
            FROM PurchaseOrder po
            WHERE (@Status IS NULL OR po.status = @Status)
            ORDER BY po.created_date DESC
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<PurchaseOrderSummaryDto>(sql, new
        {
            Status = status,
            Offset = (page - 1) * pageSize,
            PageSize = pageSize
        });
    }

    public async Task<int> GetCountAsync(string? status = null, CancellationToken cancellationToken = default)
    {
        const string sql = "SELECT COUNT(*) FROM PurchaseOrder WHERE (@Status IS NULL OR status = @Status)";

        using var connection = new SqlConnection(_connectionString);
        return await connection.ExecuteScalarAsync<int>(sql, new { Status = status });
    }
}
