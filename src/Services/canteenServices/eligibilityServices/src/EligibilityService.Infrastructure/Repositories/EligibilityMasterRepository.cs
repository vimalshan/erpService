using Microsoft.EntityFrameworkCore;
using EligibilityService.Domain.Entities;
using EligibilityService.Domain.Interfaces;
using EligibilityService.Infrastructure.Persistence;

namespace EligibilityService.Infrastructure.Repositories;

public class EligibilityMasterRepository : IEligibilityMasterRepository
{
    private readonly EligibilityDbContext _context;

    public EligibilityMasterRepository(EligibilityDbContext context) => _context = context;

    public Task<EligibilityMaster?> GetAsync(long canteenUnit, string shiftCode, decimal itemCode, CancellationToken ct)
        => _context.EligibilityMasters
            .FirstOrDefaultAsync(e =>
                e.CanteenUnit == canteenUnit &&
                e.ShiftCode == shiftCode &&
                e.ItemCode == itemCode, ct);

    public async Task<IEnumerable<EligibilityMaster>> GetAllAsync(long? canteenUnit, CancellationToken ct)
    {
        var query = _context.EligibilityMasters.AsQueryable();
        if (canteenUnit.HasValue) query = query.Where(e => e.CanteenUnit == canteenUnit.Value);
        return await query.ToListAsync(ct);
    }

    public async Task AddAsync(EligibilityMaster entity, CancellationToken ct)
        => await _context.EligibilityMasters.AddAsync(entity, ct);

    public void Update(EligibilityMaster entity)
        => _context.EligibilityMasters.Update(entity);

    public void Remove(EligibilityMaster entity)
        => _context.EligibilityMasters.Remove(entity);

    public Task<bool> ExistsAsync(long canteenUnit, string shiftCode, decimal itemCode, CancellationToken ct)
        => _context.EligibilityMasters
            .AnyAsync(e =>
                e.CanteenUnit == canteenUnit &&
                e.ShiftCode == shiftCode &&
                e.ItemCode == itemCode, ct);
}
