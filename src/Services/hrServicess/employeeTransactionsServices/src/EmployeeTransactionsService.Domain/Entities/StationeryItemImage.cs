using EmployeeTransactionsService.Domain.Common;
using EmployeeTransactionsService.Domain.Events;

namespace EmployeeTransactionsService.Domain.Entities;

public sealed class StationeryItemImage : BaseEntity
{
    private StationeryItemImage()
    {
    }

    public Guid ImageId { get; private set; }
    public string ItemReference { get; private set; } = string.Empty;
    public string BlobName { get; private set; } = string.Empty;
    public string ContentType { get; private set; } = string.Empty;
    public decimal UploadedBy { get; private set; }
    public DateTime UploadedOnUtc { get; private set; }

    public static StationeryItemImage Create(string itemReference, string blobName, string contentType, decimal uploadedBy)
    {
        var entity = new StationeryItemImage
        {
            ImageId = Guid.NewGuid(),
            ItemReference = itemReference,
            BlobName = blobName,
            ContentType = contentType,
            UploadedBy = uploadedBy,
            UploadedOnUtc = DateTime.UtcNow
        };

        entity.AddDomainEvent(new StationeryImageUploadedDomainEvent(entity.ImageId, itemReference, blobName));
        return entity;
    }
}