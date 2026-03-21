using ConfigService.Domain.Common;

namespace ConfigService.Domain.Entities;

public class ExpenseType : AggregateRoot<long>
{
    public string ExpenseName { get; private set; } = string.Empty;
    public int ExpenseCategoryId { get; private set; }
    public string TravelType { get; private set; } = string.Empty;
    public long SortNo { get; private set; }

    private ExpenseType() { }

    public static ExpenseType Create(long id, string name, int categoryId, string travelType, long sortNo)
    {
        return new ExpenseType
        {
            Id = id,
            ExpenseName = name,
            ExpenseCategoryId = categoryId,
            TravelType = travelType,
            SortNo = sortNo
        };
    }

    public void Update(string name, int categoryId, string travelType, long sortNo)
    {
        ExpenseName = name;
        ExpenseCategoryId = categoryId;
        TravelType = travelType;
        SortNo = sortNo;
    }
}
