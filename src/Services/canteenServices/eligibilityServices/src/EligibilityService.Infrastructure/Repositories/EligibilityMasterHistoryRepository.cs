using Microsoft.EntityFrameworkCore;
using EligibilityService.Domain.Entities;
using EligibilityService.Domain.Interfaces;
using EligibilityService.Infrastructure.Persistence;

namespace EligibilityService.Infrastructure.Repositories;

public class EligibilityMasterHistoryRepository : IEligibilityMasterHistoryRepository
{
    private readonly EligibilityDbContext _context;

    public EligibilityMasterHistoryRepository(EligibilityDbContext context) => _context = context;

    public async Task AddAsync(EligibilityMasterHistory entity, CancellationToken ct)
        => await _context.EligibilityMasterHistories.AddAsync(entity, ct);

    public async Task<IEnumerable<EligibilityMasterHistory>> GetHistoryAsync(long canteenUnit, string shiftCode, decimal itemCode, CancellationToken ct)
        => await _context.EligibilityMasterHistories
            .Where(h => h.CanteenUnit == canteenUnit && h.ShiftCode == shiftCode && h.ItemCode == itemCode)
            .OrderByDescending(h => h.ModifiedOn)
            .ToListAsync(ct);
}
