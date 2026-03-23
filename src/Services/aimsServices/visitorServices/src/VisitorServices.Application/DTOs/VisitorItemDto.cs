namespace VisitorServices.Application.DTOs;

public sealed record VisitorItemDto(
    long ItemId,
    long VisitorId,
    string Description,
    int Quantity,
    string? MaterialType,
    string? Notes,
    string Status,
    DateTime EnteredOn,
    long EnteredBy);
