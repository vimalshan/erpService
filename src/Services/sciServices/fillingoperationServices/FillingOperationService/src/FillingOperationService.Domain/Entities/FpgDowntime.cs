using FillingOperationService.Domain.Common;
using FillingOperationService.Domain.Events;

namespace FillingOperationService.Domain.Entities;

public class FpgDowntime : AggregateRoot
{
    public int FpgId { get; private set; }
    public int? FillingPointGroupId { get; private set; }
    public DateTime StartDateTime { get; private set; }
    public DateTime EndDateTime { get; private set; }
    public string? NoOfFillingPoints { get; private set; }
    public string? DowntimeType { get; private set; }
    public int? SciUserIdCreated { get; private set; }
    public DateTime CreationDate { get; private set; }
    public int? SciUserIdModified { get; private set; }
    public DateTime? ModifiedDate { get; private set; }

    protected FpgDowntime() { }

    public static FpgDowntime Create(int? groupId, DateTime startDateTime, DateTime endDateTime, string? noOfPoints, string? downtimeType, int? createdBy)
    {
        if (endDateTime <= startDateTime)
            throw new ArgumentException("End date/time must be after start date/time.");

        var downtime = new FpgDowntime
        {
            FillingPointGroupId = groupId,
            StartDateTime = startDateTime,
            EndDateTime = endDateTime,
            NoOfFillingPoints = noOfPoints,
            DowntimeType = downtimeType,
            SciUserIdCreated = createdBy,
            CreationDate = DateTime.UtcNow
        };
        downtime.AddDomainEvent(new DowntimeRecordedEvent(downtime));
        return downtime;
    }
}
