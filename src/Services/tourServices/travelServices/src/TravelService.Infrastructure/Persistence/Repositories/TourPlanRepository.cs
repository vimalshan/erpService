using Microsoft.EntityFrameworkCore;
using TravelService.Domain.Entities.TourPlan;
using TravelService.Domain.Repositories;
using TravelService.Infrastructure.Persistence;

namespace TravelService.Infrastructure.Persistence.Repositories;

public class TourPlanRepository : ITourPlanRepository
{
    private readonly TravelDbContext _context;

    public TourPlanRepository(TravelDbContext context) => _context = context;

    public async Task<TourPlan?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        => await _context.TourPlans
            .Include(t => t.Advances)
            .Include(t => t.Agendas)
            .Include(t => t.CostCentres)
            .Include(t => t.DaBreaks)
            .Include(t => t.Expenses)
            .Include(t => t.IntSchedules)
            .Include(t => t.Leaves)
            .Include(t => t.NmsSchedules)
            .Include(t => t.SelfExpenses)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

    public async Task<IEnumerable<TourPlan>> GetByEmployeeAsync(string employeeSysId, CancellationToken cancellationToken = default)
        => await _context.TourPlans
            .Where(t => t.EmployeeSysId == employeeSysId)
            .OrderByDescending(t => t.CreatedOn)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<TourPlan>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        => await _context.TourPlans
            .OrderByDescending(t => t.CreatedOn)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

    public async Task<TourPlan> AddAsync(TourPlan tourPlan, CancellationToken cancellationToken = default)
    {
        await _context.TourPlans.AddAsync(tourPlan, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return tourPlan;
    }

    public async Task UpdateAsync(TourPlan tourPlan, CancellationToken cancellationToken = default)
    {
        _context.TourPlans.Update(tourPlan);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
        => await _context.TourPlans.CountAsync(cancellationToken);
}
