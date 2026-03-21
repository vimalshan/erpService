using Microsoft.EntityFrameworkCore;
using AdminService.Domain.Entities;
using AdminService.Domain.Interfaces;

namespace AdminService.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for AdminUnit entity
/// </summary>
public class AdminUnitRepository : IAdminUnitRepository
{
    private readonly Persistence.AdminServiceDbContext _context;

    public AdminUnitRepository(Persistence.AdminServiceDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<AdminUnit?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.AdminUnits
            .Include(a => a.AccessConfigurations)
            .Include(a => a.ContactDetails)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<AdminUnit?> GetByAdminCodeAsync(long adminCode, CancellationToken cancellationToken = default)
    {
        return await _context.AdminUnits
            .Include(a => a.AccessConfigurations)
            .Include(a => a.ContactDetails)
            .FirstOrDefaultAsync(a => a.AdminCode == adminCode, cancellationToken);
    }

    public async Task<IEnumerable<AdminUnit>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.AdminUnits
            .Include(a => a.AccessConfigurations)
            .Include(a => a.ContactDetails)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<AdminUnit>> GetByTypeAsync(string adminType, CancellationToken cancellationToken = default)
    {
        return await _context.AdminUnits
            .Where(a => a.AdminType == adminType)
            .Include(a => a.AccessConfigurations)
            .Include(a => a.ContactDetails)
            .ToListAsync(cancellationToken);
    }

    public async Task<AdminUnit> AddAsync(AdminUnit adminUnit, CancellationToken cancellationToken = default)
    {
        await _context.AdminUnits.AddAsync(adminUnit, cancellationToken);
        return adminUnit;
    }

    public async Task<AdminUnit> UpdateAsync(AdminUnit adminUnit, CancellationToken cancellationToken = default)
    {
        _context.AdminUnits.Update(adminUnit);
        return adminUnit;
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var adminUnit = await GetByIdAsync(id, cancellationToken);
        if (adminUnit != null)
        {
            adminUnit.IsDeleted = true;
            adminUnit.DeletedAt = DateTime.UtcNow;
            _context.AdminUnits.Update(adminUnit);
        }
    }
}
