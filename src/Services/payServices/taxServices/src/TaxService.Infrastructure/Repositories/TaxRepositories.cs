using Microsoft.EntityFrameworkCore;
using TaxService.Domain.Entities;
using TaxService.Domain.Repositories;
using TaxService.Infrastructure.Data;

namespace TaxService.Infrastructure.Repositories;

public class TaxMarginalDetailRepository : ITaxMarginalDetailRepository
{
    private readonly TaxServiceDbContext _context;

    public TaxMarginalDetailRepository(TaxServiceDbContext context)
    {
        _context = context;
    }

    public async Task<TaxMarginalDetail?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.TaxMarginalDetails
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);
    }

    public async Task<TaxMarginalDetail?> GetByEmployeeAndYearAsync(
        long employeeSystemId,
        int financialYear,
        CancellationToken cancellationToken = default)
    {
        return await _context.TaxMarginalDetails
            .FirstOrDefaultAsync(
                x => x.EmployeeSystemId == employeeSystemId 
                     && x.FinancialYear == financialYear 
                     && !x.IsDeleted,
                cancellationToken);
    }

    public async Task<IEnumerable<TaxMarginalDetail>> GetByEmployeeAsync(
        long employeeSystemId,
        CancellationToken cancellationToken = default)
    {
        return await _context.TaxMarginalDetails
            .Where(x => x.EmployeeSystemId == employeeSystemId && !x.IsDeleted)
            .OrderByDescending(x => x.FinancialYear)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(TaxMarginalDetail entity, CancellationToken cancellationToken = default)
    {
        await _context.TaxMarginalDetails.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(TaxMarginalDetail entity, CancellationToken cancellationToken = default)
    {
        _context.TaxMarginalDetails.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity != null)
        {
            entity.IsDeleted = true;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}

public class ConditionalMasterRepository : IConditionalMasterRepository
{
    private readonly TaxServiceDbContext _context;

    public ConditionalMasterRepository(TaxServiceDbContext context)
    {
        _context = context;
    }

    public async Task<ConditionalMaster?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.ConditionalMasters
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);
    }

    public async Task<ConditionalMaster?> GetByPayeeIdAsync(
        string payeeId,
        int? financialYear = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.ConditionalMasters
            .Where(x => x.PayeeId == payeeId && !x.IsDeleted);

        if (financialYear.HasValue)
        {
            query = query.Where(x => x.FinancialYear == financialYear);
        }

        return await query
            .OrderByDescending(x => x.FinancialYear)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<ConditionalMaster>> GetActiveAsync(
        int? financialYear = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.ConditionalMasters
            .Where(x => x.IsActive && !x.IsDeleted);

        if (financialYear.HasValue)
        {
            query = query.Where(x => x.FinancialYear == financialYear);
        }

        return await query
            .OrderBy(x => x.PayeeId)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(ConditionalMaster entity, CancellationToken cancellationToken = default)
    {
        await _context.ConditionalMasters.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(ConditionalMaster entity, CancellationToken cancellationToken = default)
    {
        _context.ConditionalMasters.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity != null)
        {
            entity.IsDeleted = true;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
