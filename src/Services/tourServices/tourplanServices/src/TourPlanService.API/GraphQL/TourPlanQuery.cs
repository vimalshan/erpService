using HotChocolate;
using HotChocolate.Data;
using Microsoft.EntityFrameworkCore;
using TourPlanService.Domain.Entities;
using TourPlanService.Infrastructure.Data;

namespace TourPlanService.API.GraphQL;

public sealed class TourPlanQuery
{
    [UseFiltering]
    [UseSorting]
    public IQueryable<TourPlan> GetTourPlans([Service] TourPlanDbContext context) =>
        context.TourPlans.AsQueryable();

    public async Task<TourPlan?> GetTourPlanById(string tpId, [Service] TourPlanDbContext context) =>
        await context.TourPlans
            .Include(x => x.Advances)
            .Include(x => x.Agendas)
            .FirstOrDefaultAsync(x => x.TpId == tpId);

    [UseFiltering]
    [UseSorting]
    public IQueryable<ForexRequisition> GetForexRequisitions([Service] TourPlanDbContext context) =>
        context.ForexRequisitions.AsQueryable();
}
