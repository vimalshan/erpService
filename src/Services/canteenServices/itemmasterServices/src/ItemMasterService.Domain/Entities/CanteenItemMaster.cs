using ItemMasterService.Domain.Common;
using ItemMasterService.Domain.Events;
using ItemMasterService.Domain.ValueObjects;

namespace ItemMasterService.Domain.Entities;

/// <summary>Maps to CANTEEN_ITEM_MASTER table.</summary>
public class CanteenItemMaster : AggregateRoot
{
    public long CanteenUnitCode { get; private set; }   // CN_COM_COD
    public long ItemCode { get; private set; }           // CN_ITM_COD
    public string? ItemDescription { get; private set; } // CN_ITM_DES
    public string? ItemType { get; private set; }        // CN_ITM_TYP (1 char)
    public string? ItemReference { get; private set; }   // CN_ITM_REF
    public DateTime? EnteredOn { get; private set; }     // CN_ENT_DAT
    public string? EnteredBy { get; private set; }       // CN_ENT_USR

    // Navigation
    public ICollection<CanteenItemPriceMaster> PriceMasters { get; private set; } = new List<CanteenItemPriceMaster>();

    // EF constructor
    private CanteenItemMaster() { }

    public static CanteenItemMaster Create(
        long canteenUnitCode,
        long itemCode,
        string? itemDescription,
        string? itemType,
        string? itemReference,
        string enteredBy)
    {
        var entity = new CanteenItemMaster
        {
            CanteenUnitCode = canteenUnitCode,
            ItemCode = itemCode,
            ItemDescription = itemDescription?.Trim().Length > 50 ? itemDescription.Trim()[..50] : itemDescription?.Trim(),
            ItemType = SetSingleChar(itemType),
            ItemReference = itemReference?.Trim().Length > 10 ? itemReference.Trim()[..10] : itemReference?.Trim(),
            EnteredOn = DateTime.UtcNow,
            EnteredBy = enteredBy?.Trim().Length > 50 ? enteredBy.Trim()[..50] : enteredBy?.Trim()
        };

        entity.AddDomainEvent(new CanteenItemCreatedEvent(canteenUnitCode, itemCode, itemDescription));
        entity.IncrementVersion();
        return entity;
    }

    public void Update(string? itemDescription, string? itemType, string? itemReference)
    {
        ItemDescription = itemDescription?.Trim().Length > 50 ? itemDescription.Trim()[..50] : itemDescription?.Trim();
        ItemType = SetSingleChar(itemType);
        ItemReference = itemReference?.Trim().Length > 10 ? itemReference.Trim()[..10] : itemReference?.Trim();
        UpdatedAt = DateTime.UtcNow;
        AddDomainEvent(new CanteenItemUpdatedEvent(CanteenUnitCode, ItemCode));
        IncrementVersion();
    }

    private static string? SetSingleChar(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return null;
        return value.Trim()[..1];
    }
}
