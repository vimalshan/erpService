using ConfigService.Domain.Common;

namespace ConfigService.Domain.Entities;

public class GradeCatExpenseRule : AggregateRoot<string>
{
    public string GradeCategory { get; private set; } = string.Empty;
    public string ApplyToUnit { get; private set; } = string.Empty;
    public string UnitId { get; private set; } = string.Empty;
    public string ApplyToGrade { get; private set; } = string.Empty;
    public string GradeId { get; private set; } = string.Empty;
    public string ExpenseType { get; private set; } = string.Empty;
    public string Limit { get; private set; } = string.Empty;
    public string DayLimit { get; private set; } = string.Empty;
    public string BrokenFlag { get; private set; } = string.Empty;
    public string? RuleType { get; private set; }

    private readonly List<GradeCatExpenseRuleBreak> _breaks = [];
    public IReadOnlyCollection<GradeCatExpenseRuleBreak> Breaks => _breaks.AsReadOnly();

    private GradeCatExpenseRule() { }

    public static GradeCatExpenseRule Create(string id, string gradeCategory, string applyToUnit,
        string unitId, string applyToGrade, string gradeId, string expenseType,
        string limit, string dayLimit, string brokenFlag, string? ruleType)
    {
        return new GradeCatExpenseRule
        {
            Id = id, GradeCategory = gradeCategory, ApplyToUnit = applyToUnit,
            UnitId = unitId, ApplyToGrade = applyToGrade, GradeId = gradeId,
            ExpenseType = expenseType, Limit = limit, DayLimit = dayLimit,
            BrokenFlag = brokenFlag, RuleType = ruleType
        };
    }

    public void AddBreak(GradeCatExpenseRuleBreak ruleBreak) => _breaks.Add(ruleBreak);
}
