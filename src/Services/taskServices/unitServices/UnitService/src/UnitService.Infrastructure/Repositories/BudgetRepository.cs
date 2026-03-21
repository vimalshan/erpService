using Microsoft.EntityFrameworkCore;
using UnitService.Domain.Entities;
using UnitService.Domain.Interfaces;
using UnitService.Infrastructure.Data;

namespace UnitService.Infrastructure.Repositories;

public class BudgetRepository : IBudgetRepository
{
    private readonly UnitDbContext _context;

    public BudgetRepository(UnitDbContext context) => _context = context;

    public async Task<BudgetMaster?> GetByUnitCodeAsync(string unitCode, CancellationToken ct = default)
        => await _context.BudgetMasters.FirstOrDefaultAsync(b => b.UnitCode == Domain.ValueObjects.UnitCode.From(unitCode), ct);

    public async Task<IEnumerable<BudgetMaster>> GetAllAsync(CancellationToken ct = default)
        => await _context.BudgetMasters.ToListAsync(ct);

    public async Task AddAsync(BudgetMaster budget, CancellationToken ct = default)
        => await _context.BudgetMasters.AddAsync(budget, ct);

    public void Update(BudgetMaster budget)
        => _context.BudgetMasters.Update(budget);
}
