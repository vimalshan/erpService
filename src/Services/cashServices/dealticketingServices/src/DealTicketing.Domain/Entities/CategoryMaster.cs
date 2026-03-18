using DealTicketing.Domain.Common;

namespace DealTicketing.Domain.Entities;

/// <summary>Deal type categories (FX, Derivatives, Swaps).</summary>
public class CategoryMaster : BaseEntity
{
    public long CategoryId { get; private set; }
    public string CategoryName { get; private set; } = default!;
    public char CategoryType { get; private set; }   // F=FX, D=Derivatives, S=Swaps
    public DateTime CategoryModifiedOn { get; private set; }
    public decimal CategoryModifiedBy { get; private set; }

    private CategoryMaster() { }

    public CategoryMaster(long id, string name, char type, decimal modifiedBy)
    {
        CategoryId = id;
        CategoryName = name;
        CategoryType = type;
        CategoryModifiedBy = modifiedBy;
        CategoryModifiedOn = DateTime.UtcNow;
    }
}
