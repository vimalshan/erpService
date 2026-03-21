using ConfigService.Domain.Common;

namespace ConfigService.Domain.Entities;

public class GradeTypeTravelParam : AggregateRoot<string>
{
    public string GradeCategory { get; private set; } = string.Empty;
    public string ApplyToUnit { get; private set; } = string.Empty;
    public string UnitId { get; private set; } = string.Empty;
    public string AdvanceEligible { get; private set; } = string.Empty;
    public string AdvanceLimit { get; private set; } = string.Empty;
    public string AdvanceDays { get; private set; } = string.Empty;
    public string AdvanceNos { get; private set; } = string.Empty;
    public string AdvanceOut { get; private set; } = string.Empty;
    public string TpApproval { get; private set; } = string.Empty;
    public string SetTimeLimit { get; private set; } = string.Empty;

    private GradeTypeTravelParam() { }

    public static GradeTypeTravelParam Create(string id, string gradeCategory, string applyToUnit,
        string unitId, string advanceEligible, string advanceLimit, string advanceDays,
        string advanceNos, string advanceOut, string tpApproval, string setTimeLimit)
    {
        return new GradeTypeTravelParam
        {
            Id = id, GradeCategory = gradeCategory, ApplyToUnit = applyToUnit,
            UnitId = unitId, AdvanceEligible = advanceEligible, AdvanceLimit = advanceLimit,
            AdvanceDays = advanceDays, AdvanceNos = advanceNos, AdvanceOut = advanceOut,
            TpApproval = tpApproval, SetTimeLimit = setTimeLimit
        };
    }
}
