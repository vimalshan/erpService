using TimeSheetService.Domain.Common;

namespace TimeSheetService.Domain.Entities;

/// <summary>Maps to TSPROJECT_MASTER - Task Scheduling module</summary>
public class TsProject : AggregateRoot
{
    private readonly List<TsStage> _stages = new();

    public string ProjectCode { get; private set; } = string.Empty;
    public string ProjectGroup { get; private set; } = string.Empty;
    public string ProjectName { get; private set; } = string.Empty;
    public DateTime EffectiveDate { get; private set; }
    public DateTime? CloseDate { get; private set; }
    public char ProjectType { get; private set; }  // P/S/G
    public int AppId { get; private set; }
    public char ApplyAll { get; private set; }  // Y/N
    public IReadOnlyCollection<TsStage> Stages => _stages.AsReadOnly();

    private TsProject() { } // EF

    public TsProject(string projectCode, string projectGroup, string projectName,
        DateTime effectiveDate, char projectType, int appId, char applyAll, long modifiedBy)
    {
        ProjectCode = projectCode;
        ProjectGroup = projectGroup;
        ProjectName = projectName;
        EffectiveDate = effectiveDate;
        ProjectType = projectType;
        AppId = appId;
        ApplyAll = applyAll;
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;
    }
}
