using ConfigService.Domain.Common;

namespace ConfigService.Domain.Entities;

public class ExpenseGroup : AggregateRoot<string>
{
    public string GroupName { get; private set; } = string.Empty;
    public string TravelType { get; private set; } = string.Empty;
    public string BreakFlag { get; private set; } = string.Empty;

    private readonly List<ExpenseGroupMap> _mappings = [];
    public IReadOnlyCollection<ExpenseGroupMap> Mappings => _mappings.AsReadOnly();

    private ExpenseGroup() { }

    public static ExpenseGroup Create(string id, string name, string travelType, string breakFlag)
    {
        return new ExpenseGroup { Id = id, GroupName = name, TravelType = travelType, BreakFlag = breakFlag };
    }

    public void AddMapping(ExpenseGroupMap map) => _mappings.Add(map);

    public void Update(string name, string travelType, string breakFlag)
    {
        GroupName = name;
        TravelType = travelType;
        BreakFlag = breakFlag;
    }
}
