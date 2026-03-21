using ProductService.Domain.Common;

namespace ProductService.Domain.Entities;

public class Category : BaseEntity
{
    public int CategoryId { get; private set; }
    public string CategoryName { get; private set; } = null!;
    public int? ParentCategoryId { get; private set; }
    public string? Description { get; private set; }

    // Navigation properties
    public Category? ParentCategory { get; private set; }
    public ICollection<Category> SubCategories { get; private set; } = [];
    public ICollection<Product> Products { get; private set; } = [];

    private Category() { }

    public Category(string categoryName, string? description, int? parentCategoryId = null)
    {
        CategoryName = categoryName ?? throw new ArgumentNullException(nameof(categoryName));
        Description = description;
        ParentCategoryId = parentCategoryId;
    }

    public void Update(string categoryName, string? description, int? parentCategoryId)
    {
        CategoryName = categoryName ?? throw new ArgumentNullException(nameof(categoryName));
        Description = description;
        ParentCategoryId = parentCategoryId;
    }
}
