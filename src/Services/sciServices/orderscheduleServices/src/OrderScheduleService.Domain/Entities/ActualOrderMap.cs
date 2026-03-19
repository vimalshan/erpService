namespace OrderScheduleService.Domain.Entities;

using OrderScheduleService.Domain.Common;

public class ActualOrderMap : Entity
{
    public decimal? TiedOrderDetailId { get; set; }
    public decimal? ActualLineId { get; set; }
    public int? MappingQuantity { get; set; }
    public int? SciUserIdModified { get; set; }
    public DateTime? ModifiedDate { get; set; }

    public ActualOrderMap() { }

    public ActualOrderMap(
        decimal? tiedOrderDetailId,
        decimal? actualLineId,
        int? mappingQuantity,
        int? sciUserIdModified)
    {
        TiedOrderDetailId = tiedOrderDetailId;
        ActualLineId = actualLineId;
        MappingQuantity = mappingQuantity;
        SciUserIdModified = sciUserIdModified;
        ModifiedDate = DateTime.UtcNow;
    }
}
