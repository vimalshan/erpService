namespace VendorService.Application.DTOs;

public sealed record TdsVendorDto(
    long? VendorId,
    string? VendorName,
    string? EmailAddress,
    string? PanNo);
