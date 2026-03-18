using InventoryManagement.Domain.Common;

namespace InventoryManagement.Domain.Events;

public sealed class ItemRegisteredEvent : DomainEvent
{
    public int ItemId { get; }
    public string OracleCode { get; }
    public string? ItemName { get; }

    public ItemRegisteredEvent(int itemId, string oracleCode, string? itemName)
    {
        ItemId = itemId;
        OracleCode = oracleCode;
        ItemName = itemName;
    }
}
