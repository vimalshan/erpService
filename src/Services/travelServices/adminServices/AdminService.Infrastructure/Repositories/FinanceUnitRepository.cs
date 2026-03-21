using Microsoft.EntityFrameworkCore;
using AdminService.Domain.Entities;
using AdminService.Domain.Interfaces;

namespace AdminService.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for FinanceUnit entity
/// </summary>
public class FinanceUnitRepository : IFinanceUnitRepository
{
    private readonly Persistence.AdminServiceDbContext _context;

    public FinanceUnitRepository(Persistence.AdminServiceDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<FinanceUnit?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.FinanceUnits
            .Include(f => f.AccessConfigurations)
            .FirstOrDefaultAsync(f => f.Id == id, cancellationToken);
    }

    public async Task<FinanceUnit?> GetByUnitIdAsync(long unitId, CancellationToken cancellationToken = default)
    {
        return await _context.FinanceUnits
            .Include(f => f.AccessConfigurations)
            .FirstOrDefaultAsync(f => f.UnitId == unitId, cancellationToken);
    }

    public async Task<IEnumerable<FinanceUnit>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.FinanceUnits
            .Include(f => f.AccessConfigurations)
            .ToListAsync(cancellationToken);
    }

    public async Task<FinanceUnit> AddAsync(FinanceUnit financeUnit, CancellationToken cancellationToken = default)
    {
        await _context.FinanceUnits.AddAsync(financeUnit, cancellationToken);
        return financeUnit;
    }

    public async Task<FinanceUnit> UpdateAsync(FinanceUnit financeUnit, CancellationToken cancellationToken = default)
    {
        _context.FinanceUnits.Update(financeUnit);
        return financeUnit;
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var financeUnit = await GetByIdAsync(id, cancellationToken);
        if (financeUnit != null)
        {
            financeUnit.IsDeleted = true;
            financeUnit.DeletedAt = DateTime.UtcNow;
            _context.FinanceUnits.Update(financeUnit);
        }
    }
}
