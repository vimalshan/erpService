using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SalesOrderService.Application.SalesOrders.DTOs;

namespace SalesOrderService.Infrastructure.Persistence.Repositories;

/// <summary>
/// Dapper-based read-model for high-performance query scenarios.
/// </summary>
public sealed class SalesOrderReadRepository(IConfiguration config)
{
    private readonly string _cs = config.GetConnectionString("SalesOrderDb")
        ?? throw new InvalidOperationException("Missing 'SalesOrderDb' connection string.");

    public async Task<IEnumerable<SalesOrderSummaryDto>> GetSummariesAsync(CancellationToken ct = default)
    {
        const string sql = """
            SELECT so_id       AS SoId,
                   so_number   AS SoNumber,
                   customer_id AS CustomerId,
                   order_date  AS OrderDate,
                   status      AS Status,
                   total_amount AS TotalAmount
            FROM   SalesOrder
            ORDER BY created_date DESC
            """;

        await using var conn = new SqlConnection(_cs);
        var results = await conn.QueryAsync<SalesOrderSummaryDto>(
            new CommandDefinition(sql, cancellationToken: ct));
        return results;
    }

    public async Task<SalesOrderDto?> GetDetailAsync(int soId, CancellationToken ct = default)
    {
        const string headerSql = """
            SELECT so_id AS SoId, so_number AS SoNumber, customer_id AS CustomerId,
                   warehouse_id AS WarehouseId, order_date AS OrderDate,
                   requested_date AS RequestedDate, status AS Status,
                   total_amount AS TotalAmount, notes AS Notes,
                   created_by AS CreatedBy, created_date AS CreatedDate,
                   modified_date AS ModifiedDate
            FROM SalesOrder WHERE so_id = @soId
            """;
        const string linesSql = """
            SELECT so_line_id AS SoLineId, so_id AS SoId, product_id AS ProductId,
                   line_number AS LineNumber, quantity_ordered AS QuantityOrdered,
                   quantity_shipped AS QuantityShipped, unit_price AS UnitPrice,
                   discount AS Discount,
                   (COALESCE(unit_price,0) - COALESCE(discount,0)) * quantity_ordered AS LineTotal,
                   notes AS Notes
            FROM SalesOrderLine WHERE so_id = @soId ORDER BY line_number
            """;

        await using var conn = new SqlConnection(_cs);
        var header = await conn.QuerySingleOrDefaultAsync<dynamic>(
            new CommandDefinition(headerSql, new { soId }, cancellationToken: ct));
        if (header is null) return null;

        var lines = (await conn.QueryAsync<SalesOrderLineDto>(
            new CommandDefinition(linesSql, new { soId }, cancellationToken: ct))).ToList();

        return new SalesOrderDto
        {
            SoId          = (int)header.SoId,
            SoNumber      = (string)header.SoNumber,
            CustomerId    = (int)header.CustomerId,
            WarehouseId   = (int)header.WarehouseId,
            OrderDate     = DateOnly.FromDateTime((DateTime)header.OrderDate),
            RequestedDate = header.RequestedDate is null ? null : DateOnly.FromDateTime((DateTime)header.RequestedDate),
            Status        = (string)header.Status,
            TotalAmount   = (decimal?)header.TotalAmount,
            Notes         = (string?)header.Notes,
            CreatedBy     = (string?)header.CreatedBy,
            CreatedDate   = (DateTime)header.CreatedDate,
            ModifiedDate  = (DateTime)header.ModifiedDate,
            Lines         = lines
        };
    }
}
