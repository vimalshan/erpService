namespace ProductService.Application.DTOs;

public class CategoryDto
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = null!;
    public int? ParentCategoryId { get; set; }
    public string? Description { get; set; }
    public List<CategoryDto> SubCategories { get; set; } = [];
}
