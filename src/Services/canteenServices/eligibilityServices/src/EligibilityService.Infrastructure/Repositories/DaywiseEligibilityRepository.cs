using Microsoft.EntityFrameworkCore;
using EligibilityService.Domain.Entities;
using EligibilityService.Domain.Interfaces;
using EligibilityService.Infrastructure.Persistence;

namespace EligibilityService.Infrastructure.Repositories;

public class DaywiseEligibilityRepository : IDaywiseEligibilityRepository
{
    private readonly EligibilityDbContext _context;

    public DaywiseEligibilityRepository(EligibilityDbContext context) => _context = context;

    public Task<DaywiseEligibility?> GetBySerialNumberAsync(long serialNumber, CancellationToken ct)
        => _context.DaywiseEligibilities.FirstOrDefaultAsync(d => d.SerialNumber == serialNumber, ct);

    public async Task<IEnumerable<DaywiseEligibility>> GetByEmployeeAsync(long companyCode, long employeeSysId, CancellationToken ct)
        => await _context.DaywiseEligibilities
            .Where(d => d.CompanyCode == companyCode && d.EmployeeSysId == employeeSysId)
            .ToListAsync(ct);

    public async Task<IEnumerable<DaywiseEligibility>> GetByDateAsync(long companyCode, DateTime date, CancellationToken ct)
        => await _context.DaywiseEligibilities
            .Where(d => d.CompanyCode == companyCode &&
                        d.AttendanceDate.HasValue &&
                        d.AttendanceDate.Value.Date == date.Date)
            .ToListAsync(ct);

    public async Task AddAsync(DaywiseEligibility entity, CancellationToken ct)
        => await _context.DaywiseEligibilities.AddAsync(entity, ct);

    public void Update(DaywiseEligibility entity)
        => _context.DaywiseEligibilities.Update(entity);

    public void Remove(DaywiseEligibility entity)
        => _context.DaywiseEligibilities.Remove(entity);
}
