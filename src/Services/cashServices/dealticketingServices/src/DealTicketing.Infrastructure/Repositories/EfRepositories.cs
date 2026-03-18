using DealTicketing.Domain.Entities;
using DealTicketing.Domain.Interfaces;
using DealTicketing.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DealTicketing.Infrastructure.Repositories;

public class DealBatchRepository(DealTicketingDbContext db) : IDealBatchRepository
{
    public async Task<DealBatch?> GetByIdAsync(long batchId, CancellationToken ct = default)
        => await db.DealBatches
            .Include(b => b.Bank)
            .Include(b => b.DealDetails)
            .FirstOrDefaultAsync(b => b.DealBatchId == batchId, ct);

    public async Task<IReadOnlyList<DealBatch>> GetByDateAsync(DateTime date, CancellationToken ct = default)
        => await db.DealBatches
            .Include(b => b.Bank)
            .Include(b => b.DealDetails)
            .Where(b => b.DealDate.Date == date.Date)
            .OrderByDescending(b => b.DealModifiedOn)
            .ToListAsync(ct);

    public async Task AddAsync(DealBatch batch, CancellationToken ct = default)
        => await db.DealBatches.AddAsync(batch, ct);

    public void Update(DealBatch batch)
        => db.DealBatches.Update(batch);

    public async Task<bool> ExistsAsync(long batchId, CancellationToken ct = default)
        => await db.DealBatches.AnyAsync(b => b.DealBatchId == batchId, ct);
}

public class DealDetailRepository(DealTicketingDbContext db) : IDealDetailRepository
{
    public async Task<DealDetail?> GetByIdAsync(long dealId, CancellationToken ct = default)
        => await db.DealDetails
            .Include(d => d.Bank)
            .Include(d => d.Settlements)
            .Include(d => d.Attachments)
            .Include(d => d.LoanSchedules)
            .FirstOrDefaultAsync(d => d.DealId == dealId, ct);

    public async Task<IReadOnlyList<DealDetail>> GetByBatchIdAsync(long batchId, CancellationToken ct = default)
        => await db.DealDetails
            .Include(d => d.Bank)
            .Where(d => d.DealBatchId == batchId)
            .OrderBy(d => d.DealNo)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<DealDetail>> GetPendingApprovalsAsync(CancellationToken ct = default)
        => await db.DealDetails
            .Include(d => d.DealBatch)
            .Where(d => d.DealAppStatus == 'P')
            .OrderBy(d => d.DealEntryDate)
            .ToListAsync(ct);

    public async Task AddAsync(DealDetail deal, CancellationToken ct = default)
        => await db.DealDetails.AddAsync(deal, ct);

    public void Update(DealDetail deal)
        => db.DealDetails.Update(deal);
}

public class DealSettlementRepository(DealTicketingDbContext db) : IDealSettlementRepository
{
    public async Task<DealSettlement?> GetByIdAsync(long setId, CancellationToken ct = default)
        => await db.DealSettlements
            .Include(s => s.Attachments)
            .FirstOrDefaultAsync(s => s.SetId == setId, ct);

    public async Task<IReadOnlyList<DealSettlement>> GetByDealIdAsync(long dealId, CancellationToken ct = default)
        => await db.DealSettlements
            .Where(s => s.SetDealId == dealId)
            .OrderByDescending(s => s.SetDate)
            .ToListAsync(ct);

    public async Task AddAsync(DealSettlement settlement, CancellationToken ct = default)
        => await db.DealSettlements.AddAsync(settlement, ct);

    public void Update(DealSettlement settlement)
        => db.DealSettlements.Update(settlement);
}

public class BankRepository(DealTicketingDbContext db) : IBankRepository
{
    public async Task<Bank?> GetByIdAsync(long bankId, CancellationToken ct = default)
        => await db.Banks.FirstOrDefaultAsync(b => b.BankId == bankId, ct);

    public async Task<IReadOnlyList<Bank>> GetAllActiveAsync(CancellationToken ct = default)
        => await db.Banks
            .Where(b => b.BankClsDate == null || b.BankClsDate > DateTime.UtcNow)
            .OrderBy(b => b.BankName)
            .ToListAsync(ct);

    public async Task AddAsync(Bank bank, CancellationToken ct = default)
        => await db.Banks.AddAsync(bank, ct);

    public void Update(Bank bank)
        => db.Banks.Update(bank);
}
