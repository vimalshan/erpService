using Microsoft.EntityFrameworkCore;
using CanteenTransactionService.Domain.Entities;
using CanteenTransactionService.Domain.Interfaces;
using CanteenTransactionService.Infrastructure.Persistence.EF;

namespace CanteenTransactionService.Infrastructure.Persistence.Repositories;

public class DailyAvailedRepository : IDailyAvailedRepository
{
    private readonly CanteenTransactionDbContext _db;

    public DailyAvailedRepository(CanteenTransactionDbContext db) => _db = db;

    public async Task<DailyAvailed?> GetBySerialNumberAsync(long serialNumber, CancellationToken ct = default) =>
        await _db.DailyAvaileds.FirstOrDefaultAsync(e => e.SerialNumber == serialNumber, ct);

    public async Task<IEnumerable<DailyAvailed>> GetByEmployeeAsync(long employeeSysId, string fromDate, string toDate, CancellationToken ct = default) =>
        await _db.DailyAvaileds
            .Where(e => e.EmployeeSysId == employeeSysId && e.SwipeDate != null
                && string.Compare(e.SwipeDate, fromDate) >= 0
                && string.Compare(e.SwipeDate, toDate) <= 0)
            .OrderBy(e => e.SwipeDate)
            .ToListAsync(ct);

    public async Task<IEnumerable<DailyAvailed>> GetByCompanyAndDateAsync(long companyCode, string swipeDate, CancellationToken ct = default) =>
        await _db.DailyAvaileds
            .Where(e => e.CompanyCode == companyCode && e.SwipeDate != null && e.SwipeDate.StartsWith(swipeDate))
            .OrderBy(e => e.SerialNumber)
            .ToListAsync(ct);

    public async Task<long> GetNextSerialNumberAsync(CancellationToken ct = default)
    {
        var max = await _db.DailyAvaileds.MaxAsync(e => (long?)e.SerialNumber, ct);
        return (max ?? 0) + 1;
    }

    public async Task AddAsync(DailyAvailed entity, CancellationToken ct = default) =>
        await _db.DailyAvaileds.AddAsync(entity, ct);

    public async Task<int> SaveChangesAsync(CancellationToken ct = default) =>
        await _db.SaveChangesAsync(ct);
}
