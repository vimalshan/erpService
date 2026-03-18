using FillingOperationService.Domain.Common;

namespace FillingOperationService.Domain.Entities;

public class FillingLineProductMap : Entity
{
    public int FillingLineId { get; private set; }
    public int MainProductId { get; private set; }
    public int SciUserIdModified { get; private set; }
    public DateTime ModifiedDate { get; private set; }

    protected FillingLineProductMap() { }

    public static FillingLineProductMap Create(int lineId, int productId, int modifiedBy)
    {
        return new FillingLineProductMap
        {
            FillingLineId = lineId,
            MainProductId = productId,
            SciUserIdModified = modifiedBy,
            ModifiedDate = DateTime.UtcNow
        };
    }
}
