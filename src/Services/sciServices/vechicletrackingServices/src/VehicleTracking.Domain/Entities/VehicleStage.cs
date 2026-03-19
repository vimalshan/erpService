using VehicleTracking.Domain.Common;
using VehicleTracking.Domain.Events;

namespace VehicleTracking.Domain.Entities;

public class VehicleStage : BaseEntity
{
    public long TransactionNumber { get; set; }
    public long TrackingNumber { get; set; }
    public long StageSerial { get; set; }
    public DateTime EntryDate { get; set; }
    public string? EntryUser { get; set; }
    public long EntryNumber { get; set; }
    public DateTime LeaveDate { get; set; }
    public long RoleCode { get; set; }
    public char? DecisionFlag { get; set; }
    public char CancelStatus { get; set; }
    public decimal? TimeTaken { get; set; }
    public long StageCode { get; set; }
    public string? StageComment { get; set; }
    public DateTime DeleteDate { get; set; }
    public string? DeleteUser { get; set; }
    public long DeleteNumber { get; set; }

    public StageMaster? Stage { get; set; }

    public static VehicleStage Create(long transactionNumber, long trackingNumber, long stageCode, char? stageDecision,
        string entryUser, long entryNum)
    {
        var stage = new VehicleStage
        {
            TransactionNumber = transactionNumber,
            TrackingNumber = trackingNumber,
            StageSerial = 1,
            EntryDate = DateTime.UtcNow,
            EntryUser = entryUser,
            EntryNumber = entryNum,
            LeaveDate = DateTime.UtcNow,
            RoleCode = 0,
            DecisionFlag = stageDecision,
            CancelStatus = 'N',
            StageCode = stageCode,
            DeleteDate = DateTime.UtcNow,
            DeleteUser = entryUser,
            DeleteNumber = entryNum
        };

        stage.AddDomainEvent(new VehicleStageUpdatedEvent(trackingNumber, stageCode));
        return stage;
    }
}
