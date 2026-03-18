namespace InventoryManagement.Domain.Entities;

public class GradeMaster
{
    public string GradeCode { get; set; } = default!;
    public string GradeDescription { get; set; } = default!;
    public string ProductCode { get; set; } = default!;

    public ProductMaster? Product { get; set; }
}
