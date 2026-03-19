using ProductionManagement.Domain.Interfaces;
using ProductionManagement.Infrastructure.Persistence;

namespace ProductionManagement.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ProductionManagementDbContext _context;
    private IProductionPlantRepository? _productionPlants;
    private IProductionPlanRepository? _productionPlans;
    private INormsRepository? _norms;
    private IMamProductionRepository? _mamProductions;

    public UnitOfWork(ProductionManagementDbContext context) => _context = context;

    public IProductionPlantRepository ProductionPlants =>
        _productionPlants ??= new ProductionPlantRepository(_context);

    public IProductionPlanRepository ProductionPlans =>
        _productionPlans ??= new ProductionPlanRepository(_context);

    public INormsRepository Norms =>
        _norms ??= new NormsRepository(_context);

    public IMamProductionRepository MamProductions =>
        _mamProductions ??= new MamProductionRepository(_context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
