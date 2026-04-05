using TimeSheetService.Domain.Common;

namespace TimeSheetService.Domain.Entities;

/// <summary>Maps to TCPROJECTCAT_MASTER</summary>
public class TcProjectCategory : AggregateRoot
{
    public long CategoryId => Id;
    public string CategoryName { get; private set; } = string.Empty;
    public long TeamId { get; private set; }
    public long? OldCategoryId { get; private set; }

    private TcProjectCategory() { } // EF

    public TcProjectCategory(long categoryId, string categoryName, long teamId, long modifiedBy, long? oldCategoryId = null)
    {
        Id = categoryId;
        CategoryName = categoryName;
        TeamId = teamId;
        OldCategoryId = oldCategoryId;
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;
    }
}
