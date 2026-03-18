using Microsoft.EntityFrameworkCore;
using ReferenceService.Domain;
using ReferenceService.Domain.Entities;
using ReferenceService.Domain.Interfaces;

namespace ReferenceService.Infrastructure.Repositories;

/// <summary>
/// Generic repository implementation.
/// </summary>
public class Repository<TEntity, TId> : IRepository<TEntity, TId>
    where TEntity : Entity<TId>
    where TId : notnull
{
    protected readonly DbContext _context;
    
    public Repository(DbContext context)
    {
        _context = context;
    }
    
    public async Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<TEntity>().FindAsync(new object[] { id }, cancellationToken: cancellationToken);
    }
    
    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _context.Set<TEntity>().AddAsync(entity, cancellationToken);
    }
    
    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _context.Set<TEntity>().Update(entity);
        await Task.CompletedTask;
    }
    
    public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _context.Set<TEntity>().Remove(entity);
        await Task.CompletedTask;
    }
    
    public async Task<bool> ExistsAsync(TId id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<TEntity>().AnyAsync(x => x.Id!.Equals(id), cancellationToken);
    }
}

/// <summary>
/// Specific repository for LovType aggregate.
/// </summary>
public class LovTypeRepository : Repository<LovType, int>, ILovTypeRepository
{
    private readonly ReferenceService.Infrastructure.Persistence.ReferenceDbContext _dbContext;
    
    public LovTypeRepository(ReferenceService.Infrastructure.Persistence.ReferenceDbContext context)
        : base(context)
    {
        _dbContext = context;
    }
    
    public async Task<LovType?> GetByNameAsync(string typeName, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<LovType>()
            .FirstOrDefaultAsync(x => x.TypeName == typeName, cancellationToken);
    }
    
    public async Task<List<LovType>> GetAllWithValuesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<LovType>()
            .Include(x => x.Values)
            .Where(x => x.Status == EntityStatus.Active)
            .ToListAsync(cancellationToken);
    }

    public async Task<LovType?> GetWithValuesAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<LovType>()
            .Include(x => x.Values)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}

/// <summary>
/// Specific repository for LovValue entity.
/// </summary>
public class LovValueRepository : Repository<LovValue, int>, ILovValueRepository
{
    private readonly ReferenceService.Infrastructure.Persistence.ReferenceDbContext _dbContext;
    
    public LovValueRepository(ReferenceService.Infrastructure.Persistence.ReferenceDbContext context)
        : base(context)
    {
        _dbContext = context;
    }
    
    public async Task<LovValue?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<LovValue>()
            .FirstOrDefaultAsync(x => x.Code == code, cancellationToken);
    }
    
    public async Task<List<LovValue>> GetByTypeIdAsync(int typeId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<LovValue>()
            .Where(x => x.TypeId == typeId && x.Status == EntityStatus.Active)
            .OrderBy(x => x.Sequence)
            .ToListAsync(cancellationToken);
    }
}


/// <summary>
/// Specific repository for PermissionRule aggregate.
/// </summary>
public class PermissionRuleRepository : Repository<PermissionRule, int>, IPermissionRuleRepository
{
    private readonly ReferenceService.Infrastructure.Persistence.ReferenceDbContext _dbContext;
    
    public PermissionRuleRepository(ReferenceService.Infrastructure.Persistence.ReferenceDbContext context)
        : base(context)
    {
        _dbContext = context;
    }
    
    public async Task<PermissionRule?> GetByResourceAndActionAsync(string resourceId, string action, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<PermissionRule>()
            .FirstOrDefaultAsync(x => x.ResourceId == resourceId && x.Action == action, cancellationToken);
    }
}

/// <summary>
/// Specific repository for LeaveFlag aggregate.
/// </summary>
public class LeaveFlagRepository : Repository<LeaveFlag, int>, ILeaveFlagRepository
{
    private readonly ReferenceService.Infrastructure.Persistence.ReferenceDbContext _dbContext;
    
    public LeaveFlagRepository(ReferenceService.Infrastructure.Persistence.ReferenceDbContext context)
        : base(context)
    {
        _dbContext = context;
    }
    
    public async Task<LeaveFlag?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<LeaveFlag>()
            .FirstOrDefaultAsync(x => x.Code == code, cancellationToken);
    }
}
