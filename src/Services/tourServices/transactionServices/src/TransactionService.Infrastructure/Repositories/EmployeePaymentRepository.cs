using Microsoft.EntityFrameworkCore;
using TransactionService.Domain.Entities;
using TransactionService.Domain.Interfaces;
using TransactionService.Infrastructure.Persistence;

namespace TransactionService.Infrastructure.Repositories;

public sealed class EmployeePaymentRepository : IEmployeePaymentRepository
{
    private readonly TransactionDbContext _context;

    public EmployeePaymentRepository(TransactionDbContext context) => _context = context;

    public async Task<EmployeePayment?> GetByIdAsync(long payId, CancellationToken cancellationToken = default)
        => await _context.EmployeePayments.FindAsync(new object[] { payId }, cancellationToken);

    public async Task<IEnumerable<EmployeePayment>> GetByEmployeeIdAsync(long empSysId, CancellationToken cancellationToken = default)
        => await _context.EmployeePayments
            .Where(p => p.PayEmpSysId == empSysId)
            .OrderByDescending(p => p.PayDate)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<EmployeePayment>> GetByTourPlanIdAsync(long tpId, CancellationToken cancellationToken = default)
        => await _context.EmployeePayments
            .Where(p => p.PayJvId == tpId)
            .OrderByDescending(p => p.PayDate)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(EmployeePayment payment, CancellationToken cancellationToken = default)
        => await _context.EmployeePayments.AddAsync(payment, cancellationToken);
}
