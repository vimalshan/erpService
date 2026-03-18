using InventoryManagement.Domain.Common;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Events;

namespace InventoryManagement.Domain.Aggregates;

/// <summary>
/// Product Aggregate Root encapsulating MAIN_PRODUCT_MASTER and its associated metadata.
/// </summary>
public sealed class ProductAggregate : AuditableEntity, IAggregateRoot
{
    private readonly List<DomainEvent> _domainEvents = new();

    public int ProductId { get; private set; }
    public string ProductName { get; private set; } = default!;
    public string? ProductDescription { get; private set; }
    public int UnitId { get; private set; }
    public int ProductTypeId { get; private set; }
    public int CompanyUnitId { get; private set; }
    public char? MamFlag { get; private set; }

    // Navigation
    public IReadOnlyCollection<ItemMaster> Items => _items.AsReadOnly();
    private readonly List<ItemMaster> _items = new();

    public IReadOnlyList<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    private ProductAggregate() { }

    public static ProductAggregate Create(
        string productName,
        string? productDescription,
        int unitId,
        int productTypeId,
        int companyUnitId,
        int createdBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(productName);
        if (productName.Length > 20)
            throw new ArgumentException("Product name cannot exceed 20 characters.", nameof(productName));

        var product = new ProductAggregate
        {
            ProductName = productName,
            ProductDescription = productDescription,
            UnitId = unitId,
            ProductTypeId = productTypeId,
            CompanyUnitId = companyUnitId,
            CreatedBy = createdBy,
            CreatedDate = DateTime.UtcNow
        };

        product._domainEvents.Add(new ProductCreatedEvent(product.ProductId, productName));
        return product;
    }

    public void Update(string productName, string? productDescription, int unitId, int modifiedBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(productName);
        ProductName = productName;
        ProductDescription = productDescription;
        UnitId = unitId;
        ModifiedBy = modifiedBy;
        ModifiedDate = DateTime.UtcNow.ToString("O");

        _domainEvents.Add(new ProductUpdatedEvent(ProductId, productName));
    }

    public void ClearDomainEvents() => _domainEvents.Clear();
}
