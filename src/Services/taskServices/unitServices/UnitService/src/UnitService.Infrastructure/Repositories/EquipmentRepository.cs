using Microsoft.EntityFrameworkCore;
using UnitService.Domain.Entities;
using UnitService.Domain.Interfaces;
using UnitService.Infrastructure.Data;

namespace UnitService.Infrastructure.Repositories;

public class EquipmentRepository : IEquipmentRepository
{
    private readonly UnitDbContext _context;

    public EquipmentRepository(UnitDbContext context) => _context = context;

    public async Task<EquipmentMaster?> GetByIdAsync(int equipmentId, CancellationToken ct = default)
        => await _context.EquipmentMasters
            .Include(e => e.Statuses)
            .FirstOrDefaultAsync(e => e.EquipmentId == equipmentId, ct);

    public async Task<IEnumerable<EquipmentMaster>> GetAllAsync(CancellationToken ct = default)
        => await _context.EquipmentMasters.ToListAsync(ct);

    public async Task<IEnumerable<EquipmentMaster>> GetByUnitCodeAsync(string unitCode, CancellationToken ct = default)
        => await _context.EquipmentMasters
            .Where(e => e.UnitCode == Domain.ValueObjects.UnitCode.From(unitCode))
            .ToListAsync(ct);

    public async Task AddAsync(EquipmentMaster equipment, CancellationToken ct = default)
        => await _context.EquipmentMasters.AddAsync(equipment, ct);

    public void Update(EquipmentMaster equipment)
        => _context.EquipmentMasters.Update(equipment);

    public void Delete(EquipmentMaster equipment)
        => _context.EquipmentMasters.Remove(equipment);
}
