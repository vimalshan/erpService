using Ardalis.GuardClauses;
using Ardalis.Specification.EntityFrameworkCore;
using LoanAccount.Domain.Entities;
using LoanAccount.Domain.Interfaces;
using LoanAccount.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LoanAccount.Infrastructure.Repositories;

/// <summary>
/// Base repository implementation for all entities
/// </summary>
public class BaseRepository<T> : RepositoryBase<T> where T : class
{
    protected readonly LoanAccountDbContext DbContext;

    public BaseRepository(LoanAccountDbContext dbContext) : base(dbContext)
    {
        DbContext = dbContext;
    }

    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await DbContext.AddAsync(entity, cancellationToken);
        return entity;
    }

    public void Update(T entity)
    {
        DbContext.Update(entity);
    }
}

/// <summary>
/// Repository implementation for LoanMain entity
/// </summary>
public class LoanMainRepository : BaseRepository<LoanMain>, ILoanMainRepository
{
    private readonly LoanAccountDbContext _dbContext;

    public LoanMainRepository(LoanAccountDbContext dbContext) : base(dbContext)
    {
        _dbContext = Guard.Against.Null(dbContext, nameof(dbContext));
    }

    public async Task<LoanMain?> GetByLoanNumberAsync(long loanNo, CancellationToken cancellationToken = default)
    {
        return await _dbContext.LoanMains
            .FirstOrDefaultAsync(l => l.LoanNo == loanNo, cancellationToken);
    }

    public async Task<IEnumerable<LoanMain>> GetByEmployeeAsync(long empSysId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.LoanMains
            .Where(l => l.EmpSysId == empSysId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<LoanMain>> GetByUnitAsync(long unitId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.LoanMains
            .Where(l => l.UnitId == unitId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<LoanMain>> GetActiveLoansAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.LoanMains
            .Where(l => l.LoanStatus.Status == "Active")
            .ToListAsync(cancellationToken);
    }
}

/// <summary>
/// Repository implementation for LoanInstallment entity
/// </summary>
public class LoanInstallmentRepository : BaseRepository<LoanInstallment>, ILoanInstallmentRepository
{
    private readonly LoanAccountDbContext _dbContext;

    public LoanInstallmentRepository(LoanAccountDbContext dbContext) : base(dbContext)
    {
        _dbContext = Guard.Against.Null(dbContext, nameof(dbContext));
    }

    public async Task<IEnumerable<LoanInstallment>> GetByLoanNoAsync(long loanNo, CancellationToken cancellationToken = default)
    {
        return await _dbContext.LoanInstallments
            .Where(li => li.LoanNo == loanNo)
            .OrderBy(li => li.InstallmentNo)
            .ToListAsync(cancellationToken);
    }

    public async Task<LoanInstallment?> GetByInstallmentIdAsync(long installmentId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.LoanInstallments
            .FirstOrDefaultAsync(li => li.Id == installmentId, cancellationToken);
    }

    public async Task<IEnumerable<LoanInstallment>> GetPendingInstallmentsAsync(long loanNo, CancellationToken cancellationToken = default)
    {
        return await _dbContext.LoanInstallments
            .Where(li => li.LoanNo == loanNo && li.PrincipalRecovered.Amount == 0)
            .OrderBy(li => li.InstallmentDate)
            .ToListAsync(cancellationToken);
    }
}

/// <summary>
/// Repository implementation for LoanEmployeeInterestRate entity
/// </summary>
public class LoanEmployeeInterestRateRepository : BaseRepository<LoanEmployeeInterestRate>, ILoanEmployeeInterestRateRepository
{
    private readonly LoanAccountDbContext _dbContext;

    public LoanEmployeeInterestRateRepository(LoanAccountDbContext dbContext) : base(dbContext)
    {
        _dbContext = Guard.Against.Null(dbContext, nameof(dbContext));
    }

    public async Task<LoanEmployeeInterestRate?> GetByLoanNoAsync(long loanNo, CancellationToken cancellationToken = default)
    {
        return await _dbContext.LoanEmployeeInterestRates
            .Where(leir => leir.LoanNo == loanNo && leir.ClosureDate == null)
            .OrderByDescending(leir => leir.EffectiveDate)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<LoanEmployeeInterestRate>> GetActiveRatesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.LoanEmployeeInterestRates
            .Where(leir => leir.ClosureDate == null)
            .ToListAsync(cancellationToken);
    }
}

/// <summary>
/// Repository implementation for LoanLedger entity
/// </summary>
public class LoanLedgerRepository : BaseRepository<LoanLedger>, ILoanLedgerRepository
{
    private readonly LoanAccountDbContext _dbContext;

    public LoanLedgerRepository(LoanAccountDbContext dbContext) : base(dbContext)
    {
        _dbContext = Guard.Against.Null(dbContext, nameof(dbContext));
    }

    public async Task<IEnumerable<LoanLedger>> GetByLoanNoAsync(long loanNo, CancellationToken cancellationToken = default)
    {
        return await _dbContext.LoanLedgers
            .Where(ll => ll.LoanNo == loanNo)
            .OrderByDescending(ll => ll.TransactionDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<LoanLedger>> GetByEmployeeAsync(long empSysId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.LoanLedgers
            .Where(ll => ll.EmpSysId == empSysId)
            .OrderByDescending(ll => ll.TransactionDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<decimal> GetTotalDebitsAsync(long loanNo, CancellationToken cancellationToken = default)
    {
        return await _dbContext.LoanLedgers
            .Where(ll => ll.LoanNo == loanNo && ll.DCFlag == 'D')
            .SumAsync(ll => ll.TransactionAmount.Amount, cancellationToken);
    }

    public async Task<decimal> GetTotalCreditsAsync(long loanNo, CancellationToken cancellationToken = default)
    {
        return await _dbContext.LoanLedgers
            .Where(ll => ll.LoanNo == loanNo && ll.DCFlag == 'C')
            .SumAsync(ll => ll.TransactionAmount.Amount, cancellationToken);
    }
}

/// <summary>
/// Repository implementation for LoanSettlement entity
/// </summary>
public class LoanSettlementRepository : BaseRepository<LoanSettlement>, ILoanSettlementRepository
{
    private readonly LoanAccountDbContext _dbContext;

    public LoanSettlementRepository(LoanAccountDbContext dbContext) : base(dbContext)
    {
        _dbContext = Guard.Against.Null(dbContext, nameof(dbContext));
    }

    public async Task<IEnumerable<LoanSettlement>> GetByLoanNoAsync(long loanNo, CancellationToken cancellationToken = default)
    {
        return await _dbContext.LoanSettlements
            .Where(ls => ls.LoanNo == loanNo && ls.CancelledDate == null)
            .OrderByDescending(ls => ls.RecoveryDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<LoanSettlement>> GetBetweenDatesAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        return await _dbContext.LoanSettlements
            .Where(ls => ls.RecoveryDate >= startDate && ls.RecoveryDate <= endDate && ls.CancelledDate == null)
            .OrderByDescending(ls => ls.RecoveryDate)
            .ToListAsync(cancellationToken);
    }
}
