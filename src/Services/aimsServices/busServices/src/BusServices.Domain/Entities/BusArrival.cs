using BusServices.Domain.Common;
using BusServices.Domain.Exceptions;
using BusServices.Domain.ValueObjects;

namespace BusServices.Domain.Entities;

/// <summary>Maps to BUS_ARRIVALDET table.</summary>
public sealed class BusArrival : BaseEntity
{
    public long ArrivalId { get; private set; }
    public int BusId { get; private set; }
    public DateTime ArrivalDate { get; private set; }
    public TimeOnly ArrivalTime { get; private set; }
    public ArrivalStatus Status { get; private set; } = null!;
    public string? Remarks { get; private set; }
    public long LastModifiedBy { get; private set; }
    public DateTime LastModifiedOn { get; private set; }

    private BusArrival() { }

    internal static BusArrival Record(
        long arrivalId,
        int busId,
        DateTime date,
        TimeOnly time,
        char status,
        string? remarks,
        long recordedBy)
    {
        return new BusArrival
        {
            ArrivalId = arrivalId,
            BusId = busId,
            ArrivalDate = date.Date,
            ArrivalTime = time,
            Status = ArrivalStatus.Create(status),
            Remarks = remarks,
            LastModifiedBy = recordedBy,
            LastModifiedOn = DateTime.UtcNow
        };
    }

    public void UpdateStatus(char newStatus, string? remarks, long modifiedBy)
    {
        Status = ArrivalStatus.Create(newStatus);
        Remarks = remarks;
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;
    }
}
