using CategoryAndVendorService.Domain.Common;
using CategoryAndVendorService.Domain.Events;

namespace CategoryAndVendorService.Domain.Entities;

/// <summary>
/// Entity: Sub Category Master (SUBCAT_MAST)
/// </summary>
public class SubCategory : Entity
{
    public long SubCatId { get; private set; }
    public long MainCatId { get; private set; }
    public string SubCatName { get; private set; } = null!;
    public long ModifiedBy { get; private set; }
    public DateTime ModifiedOn { get; private set; }

    public MainCategory MainCategory { get; private set; } = null!;

    private SubCategory() { }

    public static SubCategory Create(long id, long mainCatId, string name, long modifiedBy)
    {
        var subCat = new SubCategory
        {
            SubCatId = id,
            MainCatId = mainCatId,
            SubCatName = name,
            ModifiedBy = modifiedBy,
            ModifiedOn = DateTime.UtcNow
        };
        subCat.RaiseDomainEvent(new SubCategoryCreatedEvent(id, name, mainCatId));
        return subCat;
    }

    public void Update(string name, long modifiedBy)
    {
        SubCatName = name;
        ModifiedBy = modifiedBy;
        ModifiedOn = DateTime.UtcNow;
    }
}
