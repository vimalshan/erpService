namespace VendorService.Application.DTOs;

public sealed record VendorDto(
    long Id,
    long CategoryId,
    long LocationId,
    string Name,
    string? Email,
    string Address,
    long UpdatedBy,
    DateTime UpdatedOn,
    char LiveStatus);
