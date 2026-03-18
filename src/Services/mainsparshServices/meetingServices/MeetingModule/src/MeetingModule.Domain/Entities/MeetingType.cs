using MeetingModule.Domain.Common;

namespace MeetingModule.Domain.Entities;

public class MeetingType : BaseEntity
{
    public long MeetTypeId { get; private set; }
    public string MeetTypeCode { get; private set; } = null!;
    public string MeetTypeName { get; private set; } = null!;
    public string? MeetTypeDesc { get; private set; }
    public string MeetTypeStatus { get; private set; } = "A";

    // Navigation
    public ICollection<MeetingSchedule> MeetingSchedules { get; private set; } = [];

    private MeetingType() { }

    public static MeetingType Create(string code, string name, string? description, long createdBy)
    {
        return new MeetingType
        {
            MeetTypeCode = code,
            MeetTypeName = name,
            MeetTypeDesc = description,
            MeetTypeStatus = "A",
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow
        };
    }

    public void Update(string name, string? description, long updatedBy)
    {
        MeetTypeName = name;
        MeetTypeDesc = description;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
    }

    public void Activate(long updatedBy)
    {
        MeetTypeStatus = "A";
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
    }

    public void Deactivate(long updatedBy)
    {
        MeetTypeStatus = "I";
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
    }
}
