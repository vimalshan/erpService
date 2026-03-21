using TravelRequestService.Domain.Common;

namespace TravelRequestService.Domain.Entities;

public class TravelSub : BaseEntity
{
    public long RequestNumber { get; private set; }
    public long SerialNumber { get; private set; }
    public long? BookingNumber { get; private set; }
    public DateTime? ModifiedDate { get; private set; }
    public DateTime? CancelDate { get; private set; }
    public string? CancelRemarks { get; private set; }
    public long? AdditionalField1 { get; private set; }
    public string? AdditionalField2 { get; private set; }
    public string? AdditionalField3 { get; private set; }
    public bool OnDuty { get; private set; }

    private TravelSub() { }

    public static TravelSub Create(
        long requestNumber,
        long serialNumber,
        long? bookingNumber = null,
        bool onDuty = false)
    {
        return new TravelSub
        {
            RequestNumber = requestNumber,
            SerialNumber = serialNumber,
            BookingNumber = bookingNumber,
            OnDuty = onDuty
        };
    }

    public void Cancel(string? remarks)
    {
        CancelDate = DateTime.UtcNow;
        CancelRemarks = remarks;
    }
}
