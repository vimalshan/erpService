namespace ProductService.Application.DTOs;

public class CreateCategoryDto
{
    public string CategoryName { get; set; } = null!;
    public string? Description { get; set; }
    public int? ParentCategoryId { get; set; }
}
