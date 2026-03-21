using ConfigService.Domain.Common;

namespace ConfigService.Domain.Entities;

public class GradeCatModeMap : AggregateRoot<string>
{
    public string GradeCategory { get; private set; } = string.Empty;
    public string ApplyToUnit { get; private set; } = string.Empty;
    public string UnitId { get; private set; } = string.Empty;
    public string ApplyToGrade { get; private set; } = string.Empty;
    public string GradeId { get; private set; } = string.Empty;
    public string ModeId { get; private set; } = string.Empty;
    public string ClassId { get; private set; } = string.Empty;
    public string SpecialStatus { get; private set; } = string.Empty;

    private GradeCatModeMap() { }

    public static GradeCatModeMap Create(string id, string gradeCategory, string applyToUnit,
        string unitId, string applyToGrade, string gradeId, string modeId, string classId, string specialStatus)
    {
        return new GradeCatModeMap
        {
            Id = id, GradeCategory = gradeCategory, ApplyToUnit = applyToUnit,
            UnitId = unitId, ApplyToGrade = applyToGrade, GradeId = gradeId,
            ModeId = modeId, ClassId = classId, SpecialStatus = specialStatus
        };
    }
}
