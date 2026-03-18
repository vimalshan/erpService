using BookingService.Domain.Common;
using BookingService.Domain.ValueObjects;

namespace BookingService.Domain.Entities;

public class BookRecord : BaseEntity
{
    public long BookingId { get; private set; }
    public string LocationCode { get; private set; } = string.Empty;
    public string? RecDetails { get; private set; }
    public RecordStatus RecStatus { get; private set; } = RecordStatus.Active;

    private BookRecord() { }

    public static BookRecord Create(long bookingId, string locationCode, string? recDetails, long createdBy)
    {
        return new BookRecord
        {
            BookingId = bookingId,
            LocationCode = locationCode,
            RecDetails = recDetails,
            RecStatus = RecordStatus.Active,
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow
        };
    }

    public void UpdateDetails(string? recDetails, long updatedBy)
    {
        RecDetails = recDetails;
        SetUpdatedAudit(updatedBy);
    }

    public void Deactivate(long updatedBy)
    {
        RecStatus = RecordStatus.Inactive;
        SetUpdatedAudit(updatedBy);
    }
}
