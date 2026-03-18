using Microsoft.EntityFrameworkCore;
using StipendService.Domain.Entities;
using StipendService.Domain.Interfaces;
using StipendService.Infrastructure.Persistence;

namespace StipendService.Infrastructure.Repositories;

public class StipendDisbursementRepository : IStipendDisbursementRepository
{
    private readonly StipendDbContext _context;

    public StipendDisbursementRepository(StipendDbContext context) => _context = context;

    public async Task<StipendDisbursement?> GetByIdAsync(long id, CancellationToken cancellationToken = default) =>
        await _context.StipendDisbursements.FindAsync([id], cancellationToken);

    public async Task<IEnumerable<StipendDisbursement>> GetByMonthYearAsync(string monthYear, CancellationToken cancellationToken = default) =>
        await _context.StipendDisbursements
            .Where(d => d.MonthYear == monthYear)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<StipendDisbursement>> GetBySrfIdAsync(long srfId, CancellationToken cancellationToken = default) =>
        await _context.StipendDisbursements
            .Where(d => d.SrfId == srfId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public async Task AddAsync(StipendDisbursement disbursement, CancellationToken cancellationToken = default) =>
        await _context.StipendDisbursements.AddAsync(disbursement, cancellationToken);

    public async Task AddRangeAsync(IEnumerable<StipendDisbursement> disbursements, CancellationToken cancellationToken = default) =>
        await _context.StipendDisbursements.AddRangeAsync(disbursements, cancellationToken);

    public Task UpdateAsync(StipendDisbursement disbursement, CancellationToken cancellationToken = default)
    {
        _context.StipendDisbursements.Update(disbursement);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsForMonthAsync(long srfId, long stipendId, string monthYear, CancellationToken cancellationToken = default) =>
        await _context.StipendDisbursements
            .AnyAsync(d => d.SrfId == srfId && d.StipendId == stipendId && d.MonthYear == monthYear, cancellationToken);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await _context.SaveChangesAsync(cancellationToken);
}
