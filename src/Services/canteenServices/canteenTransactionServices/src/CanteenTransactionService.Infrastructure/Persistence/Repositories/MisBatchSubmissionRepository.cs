using Microsoft.EntityFrameworkCore;
using CanteenTransactionService.Domain.Entities;
using CanteenTransactionService.Domain.Interfaces;
using CanteenTransactionService.Infrastructure.Persistence.EF;

namespace CanteenTransactionService.Infrastructure.Persistence.Repositories;

public class MisBatchSubmissionRepository : IMisBatchSubmissionRepository
{
    private readonly CanteenTransactionDbContext _db;

    public MisBatchSubmissionRepository(CanteenTransactionDbContext db) => _db = db;

    public async Task<MisBatchSubmission?> GetBySerialNumberAsync(long serialNumber, CancellationToken ct = default) =>
        await _db.MisBatchSubmissions.FirstOrDefaultAsync(e => e.SerialNumber == serialNumber, ct);

    public async Task<IEnumerable<MisBatchSubmission>> GetByBatchNumberAsync(long batchNumber, CancellationToken ct = default) =>
        await _db.MisBatchSubmissions
            .Where(e => e.BatchNumber == batchNumber)
            .OrderBy(e => e.SerialNumber)
            .ToListAsync(ct);

    public async Task<IEnumerable<MisBatchSubmission>> GetPendingAsync(CancellationToken ct = default) =>
        await _db.MisBatchSubmissions
            .Where(e => e.UpdateStatus == "P")
            .OrderBy(e => e.SerialNumber)
            .ToListAsync(ct);

    public async Task<IEnumerable<MisBatchSubmission>> GetByCompanyAndDateAsync(long companyCode, DateTime fromDate, DateTime toDate, CancellationToken ct = default) =>
        await _db.MisBatchSubmissions
            .Where(e => e.CompanyCode == companyCode && e.BatchDate >= fromDate && e.BatchDate <= toDate)
            .OrderBy(e => e.BatchDate)
            .ToListAsync(ct);

    public async Task AddAsync(MisBatchSubmission entity, CancellationToken ct = default) =>
        await _db.MisBatchSubmissions.AddAsync(entity, ct);

    public void Update(MisBatchSubmission entity) => _db.MisBatchSubmissions.Update(entity);

    public async Task<int> SaveChangesAsync(CancellationToken ct = default) =>
        await _db.SaveChangesAsync(ct);
}
