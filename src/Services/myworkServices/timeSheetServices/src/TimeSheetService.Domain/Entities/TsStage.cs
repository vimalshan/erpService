using TimeSheetService.Domain.Common;

namespace TimeSheetService.Domain.Entities;

/// <summary>Maps to TSSTAGE_MASTER</summary>
public class TsStage : AggregateRoot
{
    private readonly List<TsStageEmpMap> _empMaps = new();

    public string StageCode { get; private set; } = string.Empty;
    public string StageName { get; private set; } = string.Empty;
    public string ProjectCode { get; private set; } = string.Empty;
    public IReadOnlyCollection<TsStageEmpMap> EmpMaps => _empMaps.AsReadOnly();

    private TsStage() { } // EF

    public TsStage(string stageCode, string stageName, string projectCode, long modifiedBy)
    {
        StageCode = stageCode;
        StageName = stageName;
        ProjectCode = projectCode;
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;
    }
}
