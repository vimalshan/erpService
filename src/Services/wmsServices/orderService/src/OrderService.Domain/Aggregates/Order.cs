using OrderService.Domain.Common;
using OrderService.Domain.Enums;
using OrderService.Domain.Events;
using OrderService.Domain.ValueObjects;

namespace OrderService.Domain.Aggregates;

public class Order : AggregateRoot
{
    public int OrderId { get; private set; }
    public string OrderNumber { get; private set; } = null!;
    public int CustomerId { get; private set; }
    public DateTime OrderDate { get; private set; }
    public DateTime? RequiredDate { get; private set; }
    public DateTime? ShippedDate { get; private set; }
    public OrderStatus Status { get; private set; }
    public decimal TotalAmount { get; private set; }
    public string? CreatedBy { get; private set; }
    public DateTime CreatedDate { get; private set; }
    public DateTime ModifiedDate { get; private set; }

    private readonly List<Entities.OrderItem> _items = new();
    public IReadOnlyCollection<Entities.OrderItem> Items => _items.AsReadOnly();

    private Order() { } // EF constructor

    public static Order Create(int customerId, string? createdBy, DateTime? requiredDate = null)
    {
        var order = new Order
        {
            OrderNumber = ValueObjects.OrderNumber.Generate().Value,
            CustomerId = customerId,
            OrderDate = DateTime.UtcNow,
            RequiredDate = requiredDate,
            Status = OrderStatus.New,
            TotalAmount = 0,
            CreatedBy = createdBy,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };

        order.AddDomainEvent(new OrderCreatedEvent(order));
        return order;
    }

    public void AddItem(int productId, int quantity, decimal unitPrice, decimal discount = 0, string? notes = null)
    {
        var item = new Entities.OrderItem(productId, quantity, unitPrice, discount, notes);
        _items.Add(item);
        RecalculateTotal();
        ModifiedDate = DateTime.UtcNow;
    }

    public void RemoveItem(int orderItemId)
    {
        var item = _items.FirstOrDefault(i => i.OrderItemId == orderItemId)
            ?? throw new InvalidOperationException($"Order item {orderItemId} not found.");
        _items.Remove(item);
        RecalculateTotal();
        ModifiedDate = DateTime.UtcNow;
    }

    public void Process()
    {
        if (Status != OrderStatus.New)
            throw new InvalidOperationException("Only new orders can be processed.");
        if (_items.Count == 0)
            throw new InvalidOperationException("Cannot process an order with no items.");

        Status = OrderStatus.Processing;
        ModifiedDate = DateTime.UtcNow;
        AddDomainEvent(new OrderStatusChangedEvent(OrderId, OrderStatus.New, OrderStatus.Processing));
    }

    public void Ship()
    {
        if (Status != OrderStatus.Processing)
            throw new InvalidOperationException("Only processing orders can be shipped.");

        Status = OrderStatus.Shipped;
        ShippedDate = DateTime.UtcNow;
        ModifiedDate = DateTime.UtcNow;
        AddDomainEvent(new OrderStatusChangedEvent(OrderId, OrderStatus.Processing, OrderStatus.Shipped));
    }

    public void Cancel()
    {
        if (Status == OrderStatus.Shipped)
            throw new InvalidOperationException("Shipped orders cannot be cancelled.");

        var previousStatus = Status;
        Status = OrderStatus.Cancelled;
        ModifiedDate = DateTime.UtcNow;
        AddDomainEvent(new OrderStatusChangedEvent(OrderId, previousStatus, OrderStatus.Cancelled));
    }

    private void RecalculateTotal()
    {
        TotalAmount = _items.Sum(i => i.LineTotal);
    }
}
