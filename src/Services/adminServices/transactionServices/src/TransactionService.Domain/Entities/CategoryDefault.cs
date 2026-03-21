namespace TransactionService.Domain.Entities;

using TransactionService.Domain.Common;

public sealed class CategoryDefault : Entity
{
    public long StationeryId { get; private set; }
    public long CategoryId { get; private set; }
    public long LocationId { get; private set; }
    public long ModifiedBy { get; private set; }
    public DateTime ModifiedOn { get; private set; }

    private CategoryDefault() { }

    public static CategoryDefault Create(
        long stationeryId, long categoryId, long locationId, long modifiedBy)
    {
        return new CategoryDefault
        {
            StationeryId = stationeryId,
            CategoryId = categoryId,
            LocationId = locationId,
            ModifiedBy = modifiedBy,
            ModifiedOn = DateTime.UtcNow
        };
    }

    public void Update(long categoryId, long modifiedBy)
    {
        CategoryId = categoryId;
        ModifiedBy = modifiedBy;
        ModifiedOn = DateTime.UtcNow;
    }
}
