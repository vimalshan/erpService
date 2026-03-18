namespace InventoryManagement.Application.DTOs;

public record ItemDto(
    int SciItemId,
    string OracleCode,
    int OracleItemId,
    int? MainProductId,
    string? ItemName,
    string? OracleDescription,
    string ItemType,
    int? PackageTypeId,
    int ItemUomId,
    decimal ConversionFactor,
    bool IsBulkSource,
    bool IsBulkItem,
    int? MaterialTaxClassId,
    string? ProductClass,
    string? EffectiveDate,
    string? ClosureDate,
    int? LeadTime,
    int? ItemCapacityId,
    string? ItemUsage);
