using ProductionManagement.Domain.Common;

namespace ProductionManagement.Domain.Entities;

public class ProductionPlantProductMap : BaseEntity
{
    public int ProductionPlantId { get; private set; }
    public int ProductId { get; private set; }
    public int SciUserIdCreated { get; private set; }
    public DateTime CreationDate { get; private set; }

    // Navigation
    public ProductionPlant? ProductionPlant { get; private set; }

    private ProductionPlantProductMap() { }

    public ProductionPlantProductMap(int productionPlantId, int productId, int createdBy)
    {
        ProductionPlantId = productionPlantId;
        ProductId = productId;
        SciUserIdCreated = createdBy;
        CreationDate = DateTime.UtcNow;
    }
}
