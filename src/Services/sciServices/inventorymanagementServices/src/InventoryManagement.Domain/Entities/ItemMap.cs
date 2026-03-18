namespace InventoryManagement.Domain.Entities;

public class ItemMap
{
    public long OspItemId { get; set; }
    public string OspUomCode { get; set; } = default!;
    public long ItemId { get; set; }
    public decimal UomCode { get; set; }
    public string Quantity { get; set; } = default!;
    public decimal? OracleCode { get; set; }
}
