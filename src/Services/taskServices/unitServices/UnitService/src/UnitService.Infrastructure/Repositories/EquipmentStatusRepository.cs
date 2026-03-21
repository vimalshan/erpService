using Microsoft.EntityFrameworkCore;
using UnitService.Domain.Entities;
using UnitService.Domain.Interfaces;
using UnitService.Infrastructure.Data;

namespace UnitService.Infrastructure.Repositories;

public class EquipmentStatusRepository : IEquipmentStatusRepository
{
    private readonly UnitDbContext _context;

    public EquipmentStatusRepository(UnitDbContext context) => _context = context;

    public async Task<EquipmentStatus?> GetByIdAsync(int statusId, CancellationToken ct = default)
        => await _context.EquipmentStatuses.FirstOrDefaultAsync(s => s.StatusId == statusId, ct);

    public async Task<IEnumerable<EquipmentStatus>> GetByEquipmentIdAsync(int equipmentId, CancellationToken ct = default)
        => await _context.EquipmentStatuses.Where(s => s.EquipmentId == equipmentId).ToListAsync(ct);

    public async Task AddAsync(EquipmentStatus status, CancellationToken ct = default)
        => await _context.EquipmentStatuses.AddAsync(status, ct);

    public void Update(EquipmentStatus status)
        => _context.EquipmentStatuses.Update(status);
}
