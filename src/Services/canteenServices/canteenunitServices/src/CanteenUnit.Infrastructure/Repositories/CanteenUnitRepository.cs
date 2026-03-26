using CanteenUnit.Domain.Entities;
using CanteenUnit.Domain.Interfaces;
using CanteenUnit.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CanteenUnit.Infrastructure.Repositories;

public class CanteenUnitRepository : ICanteenUnitRepository
{
    private readonly ApplicationDbContext _context;
    public CanteenUnitRepository(ApplicationDbContext context) => _context = context;

    public async Task<CanteenUnitMaster?> GetByIdAsync(decimal companyCode, CancellationToken ct)
        => await _context.CanteenUnitMasters
            .FirstOrDefaultAsync(u => u.UnComCod == companyCode, ct);

    public async Task<IEnumerable<CanteenUnitMaster>> GetAllAsync(CancellationToken ct)
        => await _context.CanteenUnitMasters
            .ToListAsync(ct);

    public async Task<CanteenUnitMaster> AddAsync(CanteenUnitMaster entity, CancellationToken ct)
    {
        await _context.CanteenUnitMasters.AddAsync(entity, ct);
        return entity;
    }

    public void Update(CanteenUnitMaster entity) => _context.CanteenUnitMasters.Update(entity);

    public void Delete(CanteenUnitMaster entity) => _context.CanteenUnitMasters.Remove(entity);

    public async Task<bool> ExistsAsync(decimal companyCode, CancellationToken ct)
        => await _context.CanteenUnitMasters.AnyAsync(u => u.UnComCod == companyCode, ct);
}
