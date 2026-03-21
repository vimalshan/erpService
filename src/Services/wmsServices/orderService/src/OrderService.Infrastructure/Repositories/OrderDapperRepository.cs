using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using OrderService.Application.DTOs;

namespace OrderService.Infrastructure.Repositories;

public class OrderDapperRepository
{
    private readonly string _connectionString;

    public OrderDapperRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("OrderDb")
            ?? throw new InvalidOperationException("Connection string 'OrderDb' not found.");
    }

    public async Task<IReadOnlyList<OrderDto>> GetOrdersPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT o.order_id AS OrderId, o.order_number AS OrderNumber, o.customer_id AS CustomerId,
                   o.order_date AS OrderDate, o.required_date AS RequiredDate, o.shipped_date AS ShippedDate,
                   o.status AS Status, o.total_amount AS TotalAmount, o.created_by AS CreatedBy,
                   o.created_date AS CreatedDate, o.modified_date AS ModifiedDate
            FROM [Order] o
            ORDER BY o.order_date DESC
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY
            """;

        await using var connection = new SqlConnection(_connectionString);
        var orders = await connection.QueryAsync<OrderDto>(
            new CommandDefinition(sql, new { Offset = (page - 1) * pageSize, PageSize = pageSize }, cancellationToken: cancellationToken));
        return orders.ToList().AsReadOnly();
    }

    public async Task<int> GetOrderCountAsync(CancellationToken cancellationToken = default)
    {
        const string sql = "SELECT COUNT(*) FROM [Order]";
        await using var connection = new SqlConnection(_connectionString);
        return await connection.ExecuteScalarAsync<int>(new CommandDefinition(sql, cancellationToken: cancellationToken));
    }

    public async Task<IReadOnlyList<OrderItemDto>> GetOrderItemsAsync(int orderId, CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT order_item_id AS OrderItemId, order_id AS OrderId, product_id AS ProductId,
                   quantity AS Quantity, unit_price AS UnitPrice, discount AS Discount, notes AS Notes,
                   (unit_price * quantity) - ISNULL(discount, 0) AS LineTotal
            FROM OrderItem
            WHERE order_id = @OrderId
            """;

        await using var connection = new SqlConnection(_connectionString);
        var items = await connection.QueryAsync<OrderItemDto>(
            new CommandDefinition(sql, new { OrderId = orderId }, cancellationToken: cancellationToken));
        return items.ToList().AsReadOnly();
    }
}
