using InventoryManagement.Domain.Common;

namespace InventoryManagement.Domain.Entities;

public class ProductMaster
{
    public string ProductCode { get; set; } = default!;
    public string ProductDescription { get; set; } = default!;
    public string OracleDescription { get; set; } = default!;
    public string? UomCode { get; set; }
}
