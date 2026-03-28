using Microsoft.EntityFrameworkCore;
using TransactionService.Domain.Entities;
using TransactionService.Domain.Interfaces;

namespace TransactionService.Infrastructure.Repositories;

public class DemandMasterRepository : IDemandMasterRepository
{
    private readonly Data.TransactionDbContext _context;

    public DemandMasterRepository(Data.TransactionDbContext context)
    {
        _context = context;
    }

    public async Task<DemandMaster?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.DemandMasters
            .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted, cancellationToken);
    }

    public async Task<IEnumerable<DemandMaster>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.DemandMasters
            .Where(a => !a.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<DemandMaster>> GetByStatusAsync(char status, CancellationToken cancellationToken = default)
    {
        return await _context.DemandMasters
            .Where(a => a.DemandStatus == status && !a.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<DemandMaster>> GetByDepartmentAsync(long departmentId, CancellationToken cancellationToken = default)
    {
        return await _context.DemandMasters
            .Where(a => a.DepartmentId == departmentId && !a.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<DemandMaster>> GetByPriorityAsync(string priority, CancellationToken cancellationToken = default)
    {
        return await _context.DemandMasters
            .Where(a => a.Priority == priority && !a.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetStatusCountAsync(char status, CancellationToken cancellationToken = default)
    {
        return await _context.DemandMasters
            .CountAsync(a => a.DemandStatus == status && !a.IsDeleted, cancellationToken);
    }

    public async Task<DemandMaster> AddAsync(DemandMaster entity, CancellationToken cancellationToken = default)
    {
        await _context.DemandMasters.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(DemandMaster entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.DemandMasters.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity != null)
        {
            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.UtcNow;
            _context.DemandMasters.Update(entity);
        }
    }
}
