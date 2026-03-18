using Microsoft.EntityFrameworkCore;
using LoanApplication.Domain.Aggregates;
using LoanApplication.Domain.Interfaces;
using LoanApplication.Infrastructure.Data;

namespace LoanApplication.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for LoanApplication aggregate
/// </summary>
public class LoanApplicationRepository : ILoanApplicationRepository
{
    private readonly LoanApplicationDbContext _context;

    public LoanApplicationRepository(LoanApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<LoanApplicationAggregate?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.LoanApplications
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<LoanApplicationAggregate>> GetByEmployeeIdAsync(long employeeId, CancellationToken cancellationToken = default)
    {
        return await _context.LoanApplications
            .AsNoTracking()
            .Where(x => x.EmployeeId == employeeId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<LoanApplicationAggregate>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.LoanApplications
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<LoanApplicationAggregate>> GetPendingAsync(CancellationToken cancellationToken = default)
    {
        return await _context.LoanApplications
            .AsNoTracking()
            .Where(x => x.Status.Value == 'P' || x.Status.Value == 'C')
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(LoanApplicationAggregate loanApplication, CancellationToken cancellationToken = default)
    {
        if (loanApplication == null)
            throw new ArgumentNullException(nameof(loanApplication));

        await _context.LoanApplications.AddAsync(loanApplication, cancellationToken);
    }

    public async Task UpdateAsync(LoanApplicationAggregate loanApplication, CancellationToken cancellationToken = default)
    {
        if (loanApplication == null)
            throw new ArgumentNullException(nameof(loanApplication));

        _context.LoanApplications.Update(loanApplication);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var loanApplication = await GetByIdAsync(id, cancellationToken);
        if (loanApplication != null)
        {
            loanApplication.GetType().GetProperty("IsDeleted")?.SetValue(loanApplication, true);
            _context.LoanApplications.Update(loanApplication);
        }
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
