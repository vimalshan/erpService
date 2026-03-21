using Microsoft.EntityFrameworkCore;
using TourPlanService.Domain.Entities;
using TourPlanService.Domain.Interfaces;
using TourPlanService.Infrastructure.Data;

namespace TourPlanService.Infrastructure.Repositories;

public sealed class TourPlanRepository(TourPlanDbContext context) : ITourPlanRepository
{
    public async Task<TourPlan?> GetByIdAsync(string tpId, CancellationToken cancellationToken = default) =>
        await context.TourPlans
            .Include(x => x.Advances)
            .Include(x => x.Agendas)
            .Include(x => x.CostCentres)
            .Include(x => x.DaBreaks)
            .Include(x => x.Expenses)
            .Include(x => x.IntSchedules)
            .Include(x => x.Leaves)
            .Include(x => x.NmsSchedules)
            .Include(x => x.SelfExpenses)
            .Include(x => x.ForexRequisitions).ThenInclude(f => f.Details)
            .Include(x => x.ForexRequisitions).ThenInclude(f => f.ChequeDetails)
            .Include(x => x.DomesticDaBreaks)
            .Include(x => x.ForeignExpenses).ThenInclude(f => f.Details).ThenInclude(d => d.Breakups)
            .FirstOrDefaultAsync(x => x.TpId == tpId, cancellationToken);

    public async Task<IEnumerable<TourPlan>> GetByEmployeeIdAsync(string empSysId, CancellationToken cancellationToken = default) =>
        await context.TourPlans
            .Where(x => x.TpEmpSysId == empSysId)
            .OrderByDescending(x => x.TpCreatedOn)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<TourPlan>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default) =>
        await context.TourPlans
            .OrderByDescending(x => x.TpCreatedOn)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

    public async Task<int> GetCountAsync(CancellationToken cancellationToken = default) =>
        await context.TourPlans.CountAsync(cancellationToken);

    public async Task AddAsync(TourPlan tourPlan, CancellationToken cancellationToken = default) =>
        await context.TourPlans.AddAsync(tourPlan, cancellationToken);

    public void Update(TourPlan tourPlan) => context.TourPlans.Update(tourPlan);

    public void Delete(TourPlan tourPlan) => context.TourPlans.Remove(tourPlan);

    public async Task<bool> ExistsAsync(string tpId, CancellationToken cancellationToken = default) =>
        await context.TourPlans.AnyAsync(x => x.TpId == tpId, cancellationToken);
}
