using BatchAndEnvelopeService.Domain.Common;

namespace BatchAndEnvelopeService.Domain.Entities;

public class ScanLotMaster : Entity<long>
{
    public long UserId { get; private set; }
    public string Status { get; private set; } = default!;
    public int DeviceNo { get; private set; }
    public DateTime? CloseDate { get; private set; }
    public DateTime? CreatedOn { get; private set; }
    public long? DeviceId { get; private set; }
    public string? ScanFlag { get; private set; }

    private ScanLotMaster() { }

    public static ScanLotMaster Create(long lotNo, long userId, int deviceNo, long? deviceId = null)
    {
        return new ScanLotMaster
        {
            Id = lotNo,
            UserId = userId,
            Status = "O",
            DeviceNo = deviceNo,
            CreatedOn = DateTime.UtcNow,
            DeviceId = deviceId,
            ScanFlag = "N"
        };
    }

    public void Close()
    {
        Status = "C";
        CloseDate = DateTime.UtcNow;
        ScanFlag = "Y";
    }
}
