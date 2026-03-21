namespace OrderService.Application.DTOs;

public record CreateOrderRequest
{
    public int CustomerId { get; init; }
    public string? CreatedBy { get; init; }
    public DateTime? RequiredDate { get; init; }
    public List<CreateOrderItemRequest> Items { get; init; } = new();
}

public record CreateOrderItemRequest
{
    public int ProductId { get; init; }
    public int Quantity { get; init; }
    public decimal UnitPrice { get; init; }
    public decimal Discount { get; init; }
    public string? Notes { get; init; }
}

public record UpdateOrderStatusRequest
{
    public string Status { get; init; } = null!;
}
