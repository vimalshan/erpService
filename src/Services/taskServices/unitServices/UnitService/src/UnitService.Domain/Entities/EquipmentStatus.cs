using UnitService.Domain.Events;

namespace UnitService.Domain.Entities;

public class EquipmentStatus : BaseEntity
{
    public int StatusId { get; private set; }
    public int EquipmentId { get; private set; }
    public string StatusDescription { get; private set; } = string.Empty;
    public string StatusCode { get; private set; } = string.Empty;
    public string StartDate { get; private set; } = string.Empty;
    public string? CloseDate { get; private set; }
    public string? Remarks { get; private set; }
    public long? Hours { get; private set; }
    public string? FilePath { get; private set; }
    public int? CreatedBy { get; private set; }
    public string? CreatedOn { get; private set; }
    public int? LastModifiedBy { get; private set; }
    public DateTime? LastModifiedOn { get; private set; }

    public EquipmentMaster Equipment { get; private set; } = null!;

    private EquipmentStatus() { }

    public static EquipmentStatus Create(int statusId, int equipmentId, string statusDesc,
        string statusCode, string? remarks, long? hours, int createdBy)
    {
        var status = new EquipmentStatus
        {
            StatusId = statusId,
            EquipmentId = equipmentId,
            StatusDescription = statusDesc,
            StatusCode = statusCode,
            StartDate = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff"),
            Remarks = remarks,
            Hours = hours,
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff"),
            LastModifiedBy = createdBy,
            LastModifiedOn = DateTime.UtcNow
        };

        status.AddDomainEvent(new EquipmentStatusChangedEvent(equipmentId, statusCode, statusDesc));
        return status;
    }

    public void Close()
    {
        CloseDate = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff");
        LastModifiedOn = DateTime.UtcNow;
    }
}
