using ConfigService.Domain.Common;

namespace ConfigService.Domain.Entities;

public class ExpenseGroupMap : BaseEntity<string>
{
    public string GroupId { get; private set; } = string.Empty;
    public string ExpenseId { get; private set; } = string.Empty;

    private ExpenseGroupMap() { }

    public static ExpenseGroupMap Create(string id, string groupId, string expenseId)
    {
        return new ExpenseGroupMap { Id = id, GroupId = groupId, ExpenseId = expenseId };
    }
}
