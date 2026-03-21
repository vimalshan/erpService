using ProductService.Domain.Common;
using ProductService.Domain.Events;

namespace ProductService.Domain.Entities;

public class Product : AggregateRoot
{
    public int ProductId { get; private set; }
    public string Sku { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public int? CategoryId { get; private set; }
    public string UnitOfMeasure { get; private set; } = "EA";
    public decimal? WeightPerUnit { get; private set; }
    public decimal? VolumePerUnit { get; private set; }
    public decimal? Price { get; private set; }
    public decimal? ReorderPoint { get; private set; }
    public decimal? ReorderQuantity { get; private set; }
    public bool IsActive { get; private set; } = true;
    public DateTime CreatedDate { get; private set; }
    public DateTime ModifiedDate { get; private set; }

    // Navigation
    public Category? Category { get; private set; }

    private Product() { }

    public Product(
        string sku,
        string name,
        string? description,
        int? categoryId,
        string unitOfMeasure,
        decimal? weightPerUnit,
        decimal? volumePerUnit,
        decimal? price,
        decimal? reorderPoint,
        decimal? reorderQuantity)
    {
        Sku = sku ?? throw new ArgumentNullException(nameof(sku));
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description;
        CategoryId = categoryId;
        UnitOfMeasure = unitOfMeasure ?? "EA";
        WeightPerUnit = weightPerUnit;
        VolumePerUnit = volumePerUnit;
        Price = price;
        ReorderPoint = reorderPoint;
        ReorderQuantity = reorderQuantity;
        IsActive = true;
        CreatedDate = DateTime.UtcNow;
        ModifiedDate = DateTime.UtcNow;

        AddDomainEvent(new ProductCreatedEvent(this));
    }

    public void Update(
        string name,
        string? description,
        int? categoryId,
        string unitOfMeasure,
        decimal? weightPerUnit,
        decimal? volumePerUnit,
        decimal? price,
        decimal? reorderPoint,
        decimal? reorderQuantity)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description;
        CategoryId = categoryId;
        UnitOfMeasure = unitOfMeasure ?? "EA";
        WeightPerUnit = weightPerUnit;
        VolumePerUnit = volumePerUnit;
        Price = price;
        ReorderPoint = reorderPoint;
        ReorderQuantity = reorderQuantity;
        ModifiedDate = DateTime.UtcNow;

        AddDomainEvent(new ProductUpdatedEvent(this));
    }

    public void Deactivate()
    {
        IsActive = false;
        ModifiedDate = DateTime.UtcNow;
        AddDomainEvent(new ProductDeactivatedEvent(ProductId, Sku));
    }

    public void Activate()
    {
        IsActive = true;
        ModifiedDate = DateTime.UtcNow;
    }
}
