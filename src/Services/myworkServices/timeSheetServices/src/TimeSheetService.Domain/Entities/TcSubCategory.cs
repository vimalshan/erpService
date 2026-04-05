using TimeSheetService.Domain.Common;

namespace TimeSheetService.Domain.Entities;

/// <summary>Maps to TCSUBCAT_MASTER</summary>
public class TcSubCategory : AggregateRoot
{
    private readonly List<TcSubCategoryEmpMap> _empMaps = new();

    public long SubCategoryId => Id;
    public string SubCategoryName { get; private set; } = string.Empty;
    public long ProjectId { get; private set; }
    public long? OldSubCategoryId { get; private set; }
    public IReadOnlyCollection<TcSubCategoryEmpMap> EmpMaps => _empMaps.AsReadOnly();

    private TcSubCategory() { } // EF

    public TcSubCategory(long subCategoryId, string subCategoryName, long projectId, long modifiedBy, long? oldSubCategoryId = null)
    {
        Id = subCategoryId;
        SubCategoryName = subCategoryName;
        ProjectId = projectId;
        OldSubCategoryId = oldSubCategoryId;
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;
    }
}
