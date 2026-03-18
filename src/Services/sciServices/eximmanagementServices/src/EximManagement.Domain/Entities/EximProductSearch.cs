using EximManagement.Domain.Common;

namespace EximManagement.Domain.Entities;

public class EximProductSearch : BaseEntity
{
    public long SearchId { get; private set; }
    public long ProductId { get; private set; }
    public string? SearchItcCode { get; private set; }
    public string? SearchText { get; private set; }
    public string? NotInText { get; private set; }
    public long? LastUpdatedBy { get; private set; }
    public DateTime LastUpdatedOn { get; private set; }

    private EximProductSearch() { }

    public static EximProductSearch Create(
        long searchId, long productId, string? itcCode,
        string? searchText, string? notInText, long? updatedBy)
    {
        return new EximProductSearch
        {
            SearchId = searchId,
            ProductId = productId,
            SearchItcCode = itcCode,
            SearchText = searchText,
            NotInText = notInText,
            LastUpdatedBy = updatedBy,
            LastUpdatedOn = DateTime.UtcNow
        };
    }
}
