using LoanService.Domain.Entities;
using LoanService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LoanService.Infrastructure.Persistence.Repositories;

public class LoanRepository : ILoanRepository
{
    private readonly LoanDbContext _context;

    public LoanRepository(LoanDbContext context) => _context = context;

    public async Task<LoanMain?> GetByIdAsync(long loanNo, CancellationToken ct = default)
    {
        return await _context.Loans
            .Include(l => l.Repayments)
            .Include(l => l.Deductions)
            .FirstOrDefaultAsync(l => l.LoanNo == loanNo, ct);
    }

    public async Task<IReadOnlyList<LoanMain>> GetByMemberIdAsync(long memberId, CancellationToken ct = default)
    {
        return await _context.Loans
            .Include(l => l.Repayments)
            .Include(l => l.Deductions)
            .Where(l => l.MemberId == memberId)
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<LoanMain>> GetActiveLoansAsync(CancellationToken ct = default)
    {
        return await _context.Loans
            .Include(l => l.Repayments)
            .Where(l => l.Status == 'A')
            .ToListAsync(ct);
    }

    public async Task AddAsync(LoanMain loan, CancellationToken ct = default)
    {
        await _context.Loans.AddAsync(loan, ct);
    }

    public void Update(LoanMain loan) => _context.Loans.Update(loan);

    public async Task<bool> ExistsAsync(long loanNo, CancellationToken ct = default)
    {
        return await _context.Loans.AnyAsync(l => l.LoanNo == loanNo, ct);
    }
}
