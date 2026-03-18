using InventoryManagement.Domain.Common;
using InventoryManagement.Domain.Events;

namespace InventoryManagement.Domain.Aggregates;

/// <summary>
/// Item Aggregate Root encapsulating ITEM_MASTER and related data.
/// </summary>
public sealed class ItemAggregate : AuditableEntity, IAggregateRoot
{
    private readonly List<DomainEvent> _domainEvents = new();

    public int SciItemId { get; private set; }
    public string OracleCode { get; private set; } = default!;
    public int OracleItemId { get; private set; }
    public int? MainProductId { get; private set; }
    public string? ItemName { get; private set; }
    public string? OracleDescription { get; private set; }
    public string ItemType { get; private set; } = default!;
    public int? PackageTypeId { get; private set; }
    public int ItemUomId { get; private set; }
    public decimal MainProductUomConversionFactor { get; private set; }
    public bool IsBulkSource { get; private set; }
    public bool IsBulkItem { get; private set; }
    public int? MaterialTaxClass { get; private set; }
    public string? ProductClass { get; private set; }
    public string? EffectiveDate { get; private set; }
    public string? ClosureDate { get; private set; }
    public int? LeadTime { get; private set; }
    public int? ItemCapacityId { get; private set; }
    public string? ItemUsage { get; private set; }
    public char? MamFlag { get; private set; }
    public char? ItemAccType { get; private set; }

    public IReadOnlyList<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    private ItemAggregate() { }

    public static ItemAggregate Create(
        string oracleCode,
        int oracleItemId,
        string itemName,
        int? mainProductId,
        string itemType,
        int itemUomId,
        decimal conversionFactor,
        bool isBulkSource,
        bool isBulkItem)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(oracleCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(itemType);

        var item = new ItemAggregate
        {
            OracleCode = oracleCode,
            OracleItemId = oracleItemId,
            ItemName = itemName,
            MainProductId = mainProductId,
            ItemType = itemType,
            ItemUomId = itemUomId,
            MainProductUomConversionFactor = conversionFactor,
            IsBulkSource = isBulkSource,
            IsBulkItem = isBulkItem
        };

        item._domainEvents.Add(new ItemRegisteredEvent(item.SciItemId, oracleCode, itemName));
        return item;
    }

    public void UpdateDetails(string itemName, string itemType, int uomId, int modifiedBy)
    {
        ItemName = itemName;
        ItemType = itemType;
        ItemUomId = uomId;
        ModifiedBy = modifiedBy;
        ModifiedDate = DateTime.UtcNow.ToString("O");

        _domainEvents.Add(new ItemUpdatedEvent(SciItemId, itemName));
    }

    public void Deactivate(string closureDate, int modifiedBy)
    {
        ClosureDate = closureDate;
        ModifiedBy = modifiedBy;
        ModifiedDate = DateTime.UtcNow.ToString("O");
    }

    public void ClearDomainEvents() => _domainEvents.Clear();
}
