using InventoryManagement.Domain.Common;

namespace InventoryManagement.Domain.Entities;

public class ItemMaster : AuditableEntity
{
    public int SciItemId { get; set; }
    public string OracleCode { get; set; } = default!;
    public int OracleItemId { get; set; }
    public int? MainProductId { get; set; }
    public string? ItemName { get; set; }
    public string? OracleDescription { get; set; }
    public string ItemType { get; set; } = default!;
    public int? PackageTypeId { get; set; }
    public int ItemUomId { get; set; }
    public decimal MainProductUomConvFactor { get; set; }
    public string IsBulkSource { get; set; } = "N";
    public char IsBulkItem { get; set; } = 'N';
    public int? MaterialTaxClassId { get; set; }
    public string? ProductClass { get; set; }
    public string? EffectiveDate { get; set; }
    public string? ClosureDate { get; set; }
    public int? LeadTime { get; set; }
    public int? ItemCapacityId { get; set; }
    public string? ItemUsage { get; set; }
    public char? MamFlag { get; set; }
    public char? ItemAccType { get; set; }

    // Navigation
    public MainProductMaster? MainProduct { get; set; }
    public PackageType? PackageType { get; set; }
    public ItemCapacity? ItemCapacity { get; set; }
    public UnitOfMeasure? UnitOfMeasure { get; set; }
    public MaterialTaxClass? MaterialTaxClassNavigation { get; set; }
}
