using Microsoft.EntityFrameworkCore;
using EligibilityService.Domain.Entities;
using EligibilityService.Domain.Interfaces;
using EligibilityService.Infrastructure.Persistence;

namespace EligibilityService.Infrastructure.Repositories;

public class ShiftMappingRepository : IShiftMappingRepository
{
    private readonly EligibilityDbContext _context;

    public ShiftMappingRepository(EligibilityDbContext context) => _context = context;

    public Task<ShiftMapping?> GetAsync(long companyCode, string shiftCode, CancellationToken ct)
        => _context.ShiftMappings
            .FirstOrDefaultAsync(s => s.CompanyCode == companyCode && s.ShiftCode == shiftCode, ct);

    public async Task<IEnumerable<ShiftMapping>> GetAllAsync(long? companyCode, CancellationToken ct)
    {
        var query = _context.ShiftMappings.AsQueryable();
        if (companyCode.HasValue) query = query.Where(s => s.CompanyCode == companyCode.Value);
        return await query.ToListAsync(ct);
    }

    public async Task AddAsync(ShiftMapping entity, CancellationToken ct)
        => await _context.ShiftMappings.AddAsync(entity, ct);

    public void Remove(ShiftMapping entity)
        => _context.ShiftMappings.Remove(entity);
}
