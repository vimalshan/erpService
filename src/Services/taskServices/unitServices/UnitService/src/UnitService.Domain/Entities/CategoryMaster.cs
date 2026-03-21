using UnitService.Domain.ValueObjects;

namespace UnitService.Domain.Entities;

public class CategoryMaster : BaseEntity
{
    public UnitCode UnitCode { get; private set; } = null!;
    public decimal CategoryId { get; private set; }
    public string CategoryName { get; private set; } = string.Empty;
    public int? LastModifiedBy { get; private set; }
    public DateTime? LastModifiedOn { get; private set; }

    private CategoryMaster() { }

    public static CategoryMaster Create(string unitCode, decimal categoryId, string categoryName, int modifiedBy)
    {
        return new CategoryMaster
        {
            UnitCode = UnitCode.From(unitCode),
            CategoryId = categoryId,
            CategoryName = categoryName,
            LastModifiedBy = modifiedBy,
            LastModifiedOn = DateTime.UtcNow
        };
    }

    public void Update(string categoryName, int modifiedBy)
    {
        CategoryName = categoryName;
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;
    }
}
