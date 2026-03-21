using ConfigService.Domain.Common;

namespace ConfigService.Domain.Entities;

public class GradeCatExpenseRuleBreak : BaseEntity<string>
{
    public string RuleId { get; private set; } = string.Empty;
    public string FromHours { get; private set; } = string.Empty;
    public string ToHours { get; private set; } = string.Empty;
    public string Amount { get; private set; } = string.Empty;

    private GradeCatExpenseRuleBreak() { }

    public static GradeCatExpenseRuleBreak Create(string id, string ruleId, string fromHrs, string toHrs, string amount)
    {
        return new GradeCatExpenseRuleBreak
        {
            Id = id, RuleId = ruleId, FromHours = fromHrs, ToHours = toHrs, Amount = amount
        };
    }
}
