using ProductionManagement.Domain.Entities;
using ProductionManagement.Infrastructure.Persistence;

namespace ProductionManagement.API.GraphQL;

public class Query
{
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<ProductionPlant> GetProductionPlants([Service] ProductionManagementDbContext context)
        => context.ProductionPlants;

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<ProductionPlan> GetProductionPlans([Service] ProductionManagementDbContext context)
        => context.ProductionPlans;

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<NormsMain> GetNorms([Service] ProductionManagementDbContext context)
        => context.NormsMain;

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<MamProductionDet> GetMamProductionDetails([Service] ProductionManagementDbContext context)
        => context.MamProductionDets;

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<ProductionPlanEntry> GetProductionPlanEntries([Service] ProductionManagementDbContext context)
        => context.ProductionPlanEntries;
}
