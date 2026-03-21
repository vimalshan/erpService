using Microsoft.EntityFrameworkCore;
using UnitService.Domain.Entities;
using UnitService.Domain.Interfaces;
using UnitService.Infrastructure.Data;

namespace UnitService.Infrastructure.Repositories;

public class AccessRepository : IAccessRepository
{
    private readonly UnitDbContext _context;

    public AccessRepository(UnitDbContext context) => _context = context;

    public async Task<AccessMaster?> GetByIdAsync(int accessId, CancellationToken ct = default)
        => await _context.AccessMasters.FirstOrDefaultAsync(a => a.AccessId == accessId, ct);

    public async Task<IEnumerable<AccessMaster>> GetByUnitCodeAsync(string unitCode, CancellationToken ct = default)
        => await _context.AccessMasters
            .Where(a => a.UnitCode == Domain.ValueObjects.UnitCode.From(unitCode))
            .ToListAsync(ct);

    public async Task<IEnumerable<AccessMaster>> GetByEmployeeAsync(int employeeSysId, CancellationToken ct = default)
        => await _context.AccessMasters.Where(a => a.EmployeeSysId == employeeSysId).ToListAsync(ct);

    public async Task<int> GetNextIdAsync(CancellationToken ct = default)
    {
        var maxId = await _context.AccessMasters.MaxAsync(a => (int?)a.AccessId, ct);
        return (maxId ?? 0) + 1;
    }

    public async Task AddAsync(AccessMaster access, CancellationToken ct = default)
        => await _context.AccessMasters.AddAsync(access, ct);

    public void Update(AccessMaster access)
        => _context.AccessMasters.Update(access);
}
