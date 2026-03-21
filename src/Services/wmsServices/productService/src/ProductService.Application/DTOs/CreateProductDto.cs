namespace ProductService.Application.DTOs;

public class CreateProductDto
{
    public string Sku { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int? CategoryId { get; set; }
    public string UnitOfMeasure { get; set; } = "EA";
    public decimal? WeightPerUnit { get; set; }
    public decimal? VolumePerUnit { get; set; }
    public decimal? Price { get; set; }
    public decimal? ReorderPoint { get; set; }
    public decimal? ReorderQuantity { get; set; }
}
