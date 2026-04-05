using TimeSheetService.Domain.Common;

namespace TimeSheetService.Domain.Entities;

/// <summary>Maps to TCPROJECT_MASTER</summary>
public class TcProject : AggregateRoot
{
    public long ProjectId => Id;
    public string ProjectName { get; private set; } = string.Empty;
    public long CategoryId { get; private set; }
    public DateTime EffectiveDate { get; private set; }
    public DateTime? CloseDate { get; private set; }
    public long TeamId { get; private set; }
    public char ListAll { get; private set; }
    public long? OldProjectId { get; private set; }

    private TcProject() { } // EF

    public TcProject(long projectId, string projectName, long categoryId, DateTime effectiveDate,
        long teamId, char listAll, long modifiedBy, long? oldProjectId = null)
    {
        Id = projectId;
        ProjectName = projectName;
        CategoryId = categoryId;
        EffectiveDate = effectiveDate;
        TeamId = teamId;
        ListAll = listAll;
        OldProjectId = oldProjectId;
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;
    }

    public void Update(string projectName, DateTime? closeDate, long modifiedBy)
    {
        ProjectName = projectName;
        CloseDate = closeDate;
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;
    }
}
