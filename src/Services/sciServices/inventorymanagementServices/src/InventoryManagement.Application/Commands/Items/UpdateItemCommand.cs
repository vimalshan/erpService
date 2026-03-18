using MediatR;

namespace InventoryManagement.Application.Commands.Items;

public record UpdateItemCommand(
    int SciItemId,
    string? ItemName,
    string ItemType,
    int ItemUomId,
    int? LeadTime,
    int ModifiedBy) : IRequest;
