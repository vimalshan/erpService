using ProductionManagement.Domain.Common;
using ProductionManagement.Domain.Events;

namespace ProductionManagement.Domain.Entities;

public class ProductionPlant : AuditableEntity, IAggregateRoot
{
    public int ProductionPlantId { get; private set; }
    public int CompanyUnitId { get; private set; }
    public string PlantName { get; private set; } = string.Empty;
    public string Location { get; private set; } = string.Empty;

    // Navigation properties
    private readonly List<ProductionPlan> _productionPlans = new();
    public IReadOnlyCollection<ProductionPlan> ProductionPlans => _productionPlans.AsReadOnly();

    private readonly List<ProductionPlantProductMap> _productMaps = new();
    public IReadOnlyCollection<ProductionPlantProductMap> ProductMaps => _productMaps.AsReadOnly();

    private ProductionPlant() { }

    public ProductionPlant(int companyUnitId, string plantName, string location, int createdBy)
    {
        CompanyUnitId = companyUnitId;
        PlantName = plantName ?? throw new ArgumentNullException(nameof(plantName));
        Location = location ?? throw new ArgumentNullException(nameof(location));
        SciUserIdCreated = createdBy;
        CreationDate = DateTime.UtcNow;

        AddDomainEvent(new ProductionPlantCreatedEvent(this));
    }

    public void Update(string plantName, string location, int modifiedBy)
    {
        PlantName = plantName ?? throw new ArgumentNullException(nameof(plantName));
        Location = location ?? throw new ArgumentNullException(nameof(location));
        SciUserIdModified = modifiedBy;
        ModifiedDate = DateTime.UtcNow;

        AddDomainEvent(new ProductionPlantUpdatedEvent(this));
    }

    public void AddProductMap(int productId, int createdBy)
    {
        if (_productMaps.Any(pm => pm.ProductId == productId))
            throw new InvalidOperationException($"Product {productId} is already mapped to this plant.");

        _productMaps.Add(new ProductionPlantProductMap(ProductionPlantId, productId, createdBy));
    }
}
