using MeetingModule.Domain.Common;
using MeetingModule.Domain.ValueObjects;

namespace MeetingModule.Domain.Entities;

public class PollDetail : BaseEntity
{
    public long PollId { get; private set; }
    public long MeetingId { get; private set; }
    public string PollQuestion { get; private set; } = null!;
    public string? PollType { get; private set; }
    public string PollStatus { get; private set; } = "ACTIVE";

    // Navigation
    public MeetingSchedule Meeting { get; private set; } = null!;

    private PollDetail() { }

    public static PollDetail Create(long meetingId, string question, string? pollType, long? createdBy)
    {
        return new PollDetail
        {
            MeetingId = meetingId,
            PollQuestion = question,
            PollType = pollType != null ? ValueObjects.PollType.From(pollType).Value : null,
            PollStatus = ValueObjects.PollStatus.Active.Value,
            CreatedBy = createdBy ?? 0,
            CreatedOn = DateTime.UtcNow
        };
    }

    public void Update(string question, string? pollType, long? updatedBy)
    {
        PollQuestion = question;
        PollType = pollType != null ? ValueObjects.PollType.From(pollType).Value : null;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
    }

    public void Close(long? updatedBy)
    {
        PollStatus = ValueObjects.PollStatus.Closed.Value;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
    }

    public void Archive(long? updatedBy)
    {
        PollStatus = ValueObjects.PollStatus.Archived.Value;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
    }
}
