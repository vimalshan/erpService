namespace OrderService.Application.DTOs;

public record OrderDto
{
    public int OrderId { get; init; }
    public string OrderNumber { get; init; } = null!;
    public int CustomerId { get; init; }
    public DateTime OrderDate { get; init; }
    public DateTime? RequiredDate { get; init; }
    public DateTime? ShippedDate { get; init; }
    public string Status { get; init; } = null!;
    public decimal TotalAmount { get; init; }
    public string? CreatedBy { get; init; }
    public DateTime CreatedDate { get; init; }
    public DateTime ModifiedDate { get; init; }
    public List<OrderItemDto> Items { get; init; } = new();
}

public record OrderItemDto
{
    public int OrderItemId { get; init; }
    public int OrderId { get; init; }
    public int ProductId { get; init; }
    public int Quantity { get; init; }
    public decimal UnitPrice { get; init; }
    public decimal Discount { get; init; }
    public string? Notes { get; init; }
    public decimal LineTotal { get; init; }
}
