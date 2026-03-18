using CanteenUnit.Domain.Entities;
using CanteenUnit.Domain.Interfaces;
using CanteenUnit.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CanteenUnit.Infrastructure.Repositories;

public class CanteenMasterRepository : ICanteenMasterRepository
{
    private readonly ApplicationDbContext _context;
    public CanteenMasterRepository(ApplicationDbContext context) => _context = context;

    public async Task<CanteenMaster?> GetByIdAsync(decimal companyCode, CancellationToken ct)
        => await _context.CanteenMasters
            .Include(m => m.Categories)
            .Include(m => m.GradeCategories)
            .FirstOrDefaultAsync(m => m.CnComCod == companyCode, ct);

    public async Task<IEnumerable<CanteenMaster>> GetAllAsync(CancellationToken ct)
        => await _context.CanteenMasters.ToListAsync(ct);

    public async Task<CanteenMaster> AddAsync(CanteenMaster entity, CancellationToken ct)
    {
        await _context.CanteenMasters.AddAsync(entity, ct);
        return entity;
    }

    public void Update(CanteenMaster entity) => _context.CanteenMasters.Update(entity);
    public void Delete(CanteenMaster entity) => _context.CanteenMasters.Remove(entity);
}
