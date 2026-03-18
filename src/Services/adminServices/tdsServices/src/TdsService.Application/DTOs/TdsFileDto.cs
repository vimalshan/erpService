namespace TdsService.Application.DTOs;

public sealed record TdsFileDto(
    long FileId,
    string FileName,
    string? PanNo,
    string EmailStatus,
    string? FileType,
    string? BlobStorageUri,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt
);
