using Microsoft.EntityFrameworkCore;
using ReimbursementService.Domain.Entities;
using ReimbursementService.Domain.Enums;
using ReimbursementService.Domain.Interfaces;
using ReimbursementService.Infrastructure.Persistence;

namespace ReimbursementService.Infrastructure.Persistence.Repositories;

public sealed class ReimbursementRepository(ApplicationDbContext context) : IReimbursementRepository
{
    public async Task<ReimbursementTransaction?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        => await context.ReimTran.FindAsync([id], cancellationToken);

    public async Task<ReimbursementTransaction?> GetByRefNoAsync(string refNo, CancellationToken cancellationToken = default)
        => await context.ReimTran
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ReimRefNo == refNo, cancellationToken);

    public async Task<IEnumerable<ReimbursementTransaction>> GetByEmployeeAsync(long empSysId, CancellationToken cancellationToken = default)
        => await context.ReimTran
            .AsNoTracking()
            .Where(x => x.EmpSysId == empSysId)
            .OrderByDescending(x => x.ReimDate)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<ReimbursementTransaction>> GetByStatusAsync(ReimbursementStatus status, CancellationToken cancellationToken = default)
        => await context.ReimTran
            .AsNoTracking()
            .Where(x => x.Status == status)
            .OrderByDescending(x => x.ReimDate)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<ReimbursementTransaction>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        => await context.ReimTran
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedOn)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

    public async Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default)
        => await context.ReimTran.CountAsync(cancellationToken);

    public async Task<ReimbursementTransaction> AddAsync(ReimbursementTransaction entity, CancellationToken cancellationToken = default)
    {
        await context.ReimTran.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(ReimbursementTransaction entity, CancellationToken cancellationToken = default)
    {
        context.ReimTran.Update(entity);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> RefNoExistsAsync(string refNo, CancellationToken cancellationToken = default)
        => await context.ReimTran.AnyAsync(x => x.ReimRefNo == refNo, cancellationToken);
}
