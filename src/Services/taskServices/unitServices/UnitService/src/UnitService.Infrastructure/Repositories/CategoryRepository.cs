using Microsoft.EntityFrameworkCore;
using UnitService.Domain.Entities;
using UnitService.Domain.Interfaces;
using UnitService.Infrastructure.Data;

namespace UnitService.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly UnitDbContext _context;

    public CategoryRepository(UnitDbContext context) => _context = context;

    public async Task<CategoryMaster?> GetByUnitCodeAsync(string unitCode, CancellationToken ct = default)
        => await _context.CategoryMasters.FirstOrDefaultAsync(c => c.UnitCode == Domain.ValueObjects.UnitCode.From(unitCode), ct);

    public async Task<IEnumerable<CategoryMaster>> GetAllAsync(CancellationToken ct = default)
        => await _context.CategoryMasters.ToListAsync(ct);

    public async Task AddAsync(CategoryMaster category, CancellationToken ct = default)
        => await _context.CategoryMasters.AddAsync(category, ct);

    public void Update(CategoryMaster category)
        => _context.CategoryMasters.Update(category);
}
