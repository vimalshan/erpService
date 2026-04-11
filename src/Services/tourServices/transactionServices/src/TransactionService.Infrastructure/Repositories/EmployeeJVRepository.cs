using Microsoft.EntityFrameworkCore;
using TransactionService.Domain.Aggregates;
using TransactionService.Domain.Interfaces;
using TransactionService.Domain.ValueObjects;
using TransactionService.Infrastructure.Persistence;

namespace TransactionService.Infrastructure.Repositories;

public sealed class EmployeeJVRepository : IEmployeeJVRepository
{
    private readonly TransactionDbContext _context;

    public EmployeeJVRepository(TransactionDbContext context) => _context = context;

    public async Task<EmployeeJournalVoucher?> GetByIdAsync(long jvBatchId, CancellationToken cancellationToken = default)
        => await _context.EmployeeJournalVouchers
            .Include(j => j.Lines)
            .FirstOrDefaultAsync(j => j.JvBatchId == jvBatchId, cancellationToken);

    public async Task<IEnumerable<EmployeeJournalVoucher>> GetByEmployeeIdAsync(long empSysId, CancellationToken cancellationToken = default)
        => await _context.EmployeeJournalVouchers
            .Include(j => j.Lines)
            .Where(j => j.JvEmpSysId == empSysId)
            .OrderByDescending(j => j.JvDate)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<EmployeeJournalVoucher>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        => await _context.EmployeeJournalVouchers
            .Include(j => j.Lines)
            .OrderByDescending(j => j.JvDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

    public async Task<int> GetCountAsync(CancellationToken cancellationToken = default)
        => await _context.EmployeeJournalVouchers.CountAsync(cancellationToken);

    public async Task AddAsync(EmployeeJournalVoucher jv, CancellationToken cancellationToken = default)
        => await _context.EmployeeJournalVouchers.AddAsync(jv, cancellationToken);

    public void Update(EmployeeJournalVoucher jv)
        => _context.EmployeeJournalVouchers.Update(jv);

    public async Task<bool> ExistsAsync(long jvBatchId, CancellationToken cancellationToken = default)
        => await _context.EmployeeJournalVouchers.AnyAsync(j => j.JvBatchId == jvBatchId, cancellationToken);
}
