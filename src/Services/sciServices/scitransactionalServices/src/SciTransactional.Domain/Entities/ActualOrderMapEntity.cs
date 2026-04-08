using SciTransactional.Domain.Common;

namespace SciTransactional.Domain.Entities;

public sealed class ActualOrderMapEntity : Entity<int>
{
    public decimal? TiedOrderDetailId { get; private set; }
    public decimal? ActualLineId { get; private set; }
    public int? MappingQuantity { get; private set; }
    public int? ModifiedByUserId { get; private set; }
    public DateTime? ModifiedDate { get; private set; }

    private ActualOrderMapEntity() { }

    public static ActualOrderMapEntity Create(
        decimal? tiedOrderDetailId, decimal? actualLineId,
        int? mappingQuantity, int? modifiedByUserId)
    {
        return new ActualOrderMapEntity
        {
            TiedOrderDetailId = tiedOrderDetailId,
            ActualLineId = actualLineId,
            MappingQuantity = mappingQuantity,
            ModifiedByUserId = modifiedByUserId,
            ModifiedDate = DateTime.UtcNow
        };
    }

    public void UpdateQuantity(int? newQuantity, int? modifiedByUserId)
    {
        MappingQuantity = newQuantity ?? MappingQuantity;
        ModifiedByUserId = modifiedByUserId;
        ModifiedDate = DateTime.UtcNow;
    }
}
