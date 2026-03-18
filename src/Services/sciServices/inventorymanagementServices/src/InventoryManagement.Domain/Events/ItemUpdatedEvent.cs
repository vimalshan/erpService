using InventoryManagement.Domain.Common;

namespace InventoryManagement.Domain.Events;

public sealed class ItemUpdatedEvent : DomainEvent
{
    public int ItemId { get; }
    public string? ItemName { get; }

    public ItemUpdatedEvent(int itemId, string? itemName)
    {
        ItemId = itemId;
        ItemName = itemName;
    }
}
