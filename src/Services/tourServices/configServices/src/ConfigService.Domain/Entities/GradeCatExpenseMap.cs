using ConfigService.Domain.Common;

namespace ConfigService.Domain.Entities;

public class GradeCatExpenseMap : AggregateRoot<string>
{
    public string GradeCategory { get; private set; } = string.Empty;
    public string ApplyToUnit { get; private set; } = string.Empty;
    public string UnitId { get; private set; } = string.Empty;
    public string ApplyToGrade { get; private set; } = string.Empty;
    public string GradeId { get; private set; } = string.Empty;
    public string ExpenseId { get; private set; } = string.Empty;

    private GradeCatExpenseMap() { }

    public static GradeCatExpenseMap Create(string id, string gradeCategory, string applyToUnit,
        string unitId, string applyToGrade, string gradeId, string expenseId)
    {
        return new GradeCatExpenseMap
        {
            Id = id, GradeCategory = gradeCategory, ApplyToUnit = applyToUnit,
            UnitId = unitId, ApplyToGrade = applyToGrade, GradeId = gradeId,
            ExpenseId = expenseId
        };
    }
}
