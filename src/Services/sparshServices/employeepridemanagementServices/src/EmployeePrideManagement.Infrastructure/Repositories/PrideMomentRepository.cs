using EmployeePrideManagement.Domain.Entities;
using EmployeePrideManagement.Domain.Interfaces;
using EmployeePrideManagement.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace EmployeePrideManagement.Infrastructure.Repositories;

public class PrideMomentRepository : IPrideMomentRepository
{
    private readonly PrideManagementDbContext _context;

    public PrideMomentRepository(PrideManagementDbContext context)
    {
        _context = context;
    }

    public async Task<MomentPride?> GetByIdAsync(decimal id, CancellationToken cancellationToken = default)
    {
        return await _context.MomentPrides.FirstOrDefaultAsync(x => x.MomentPrideId == id, cancellationToken);
    }

    public async Task<IEnumerable<MomentPride>> GetByEmployeeIdAsync(decimal employeeSysId, CancellationToken cancellationToken = default)
    {
        return await _context.MomentPrides
            .Where(x => x.EmployeeSysId == employeeSysId)
            .OrderByDescending(x => x.ModifiedOn)
            .ToListAsync(cancellationToken);
    }

    public async Task<(IEnumerable<MomentPride> Items, int TotalCount)> GetAllPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var totalCount = await _context.MomentPrides.CountAsync(cancellationToken);
        var items = await _context.MomentPrides
            .OrderByDescending(x => x.ModifiedOn)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<MomentPride> AddAsync(MomentPride entity, CancellationToken cancellationToken = default)
    {
        await _context.MomentPrides.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(MomentPride entity, CancellationToken cancellationToken = default)
    {
        _context.MomentPrides.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(decimal id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity is not null)
        {
            _context.MomentPrides.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
