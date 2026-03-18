namespace VendorService.Application.DTOs;

public sealed record TdsFileDetailDto(
    long FileId,
    string? FileName,
    string? PanNo,
    string? EmailStatus,
    string? FileType);
