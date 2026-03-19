namespace ProductionManagement.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IProductionPlantRepository ProductionPlants { get; }
    IProductionPlanRepository ProductionPlans { get; }
    INormsRepository Norms { get; }
    IMamProductionRepository MamProductions { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
