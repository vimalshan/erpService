using SciTransactional.Domain.Common;

namespace SciTransactional.Domain.Entities;

public sealed class VehicleDirectEntryEntity : Entity<long>
{
    public long? TrackingNumber { get; private set; }
    public DateTime? EnteredDate { get; private set; }
    public string? EnteredUser { get; private set; }

    private VehicleDirectEntryEntity() { }

    public static VehicleDirectEntryEntity Create(
        long? trackingNumber, string? enteredUser)
    {
        return new VehicleDirectEntryEntity
        {
            TrackingNumber = trackingNumber,
            EnteredDate = DateTime.UtcNow,
            EnteredUser = enteredUser
        };
    }
}
