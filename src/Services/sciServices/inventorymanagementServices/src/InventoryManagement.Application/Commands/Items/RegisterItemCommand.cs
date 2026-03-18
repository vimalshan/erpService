using InventoryManagement.Application.DTOs;
using MediatR;

namespace InventoryManagement.Application.Commands.Items;

public record RegisterItemCommand(
    string OracleCode,
    int OracleItemId,
    string? ItemName,
    int? MainProductId,
    string ItemType,
    int ItemUomId,
    decimal ConversionFactor,
    bool IsBulkSource,
    bool IsBulkItem,
    int? PackageTypeId,
    int? MaterialTaxClassId,
    int? LeadTime) : IRequest<ItemDto>;
