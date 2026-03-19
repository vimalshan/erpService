using CategoryAndVendorService.Domain.Common;
using CategoryAndVendorService.Domain.Events;

namespace CategoryAndVendorService.Domain.Entities;

/// <summary>
/// Aggregate Root: Main Category Master (MAINCAT_MAST)
/// </summary>
public class MainCategory : Entity
{
    public long MainCatId { get; private set; }
    public string MainCatName { get; private set; } = null!;
    public long MainCatPriority { get; private set; }
    public long ModifiedBy { get; private set; }
    public DateTime ModifiedOn { get; private set; }
    public long? DefaultSubCatId { get; private set; }
    public long? AvgResponseTime { get; private set; }

    private readonly List<SubCategory> _subCategories = new();
    public IReadOnlyCollection<SubCategory> SubCategories => _subCategories.AsReadOnly();

    private MainCategory() { }

    public static MainCategory Create(long id, string name, long priority, long modifiedBy, long? defaultSubCatId = null, long? avgResponseTime = null)
    {
        var category = new MainCategory
        {
            MainCatId = id,
            MainCatName = name,
            MainCatPriority = priority,
            ModifiedBy = modifiedBy,
            ModifiedOn = DateTime.UtcNow,
            DefaultSubCatId = defaultSubCatId,
            AvgResponseTime = avgResponseTime
        };
        category.RaiseDomainEvent(new MainCategoryCreatedEvent(id, name));
        return category;
    }

    public void Update(string name, long priority, long modifiedBy, long? defaultSubCatId, long? avgResponseTime)
    {
        MainCatName = name;
        MainCatPriority = priority;
        ModifiedBy = modifiedBy;
        ModifiedOn = DateTime.UtcNow;
        DefaultSubCatId = defaultSubCatId;
        AvgResponseTime = avgResponseTime;
        RaiseDomainEvent(new MainCategoryUpdatedEvent(MainCatId, name));
    }

    public void AddSubCategory(SubCategory subCategory)
    {
        _subCategories.Add(subCategory);
    }
}
