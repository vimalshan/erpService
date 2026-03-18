namespace InventoryManagement.Application.DTOs;

public record UploadItemImageRequest(
    int ItemId,
    string FileName,
    string ContentType,
    Stream Content);

public record UploadItemImageResponse(
    int ItemId,
    string BlobUrl);
