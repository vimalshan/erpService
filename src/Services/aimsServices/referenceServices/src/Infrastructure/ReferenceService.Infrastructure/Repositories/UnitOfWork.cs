using ReferenceService.Domain.Interfaces;
using ReferenceService.Infrastructure.Persistence;

namespace ReferenceService.Infrastructure.Repositories;

/// <summary>
/// Unit of Work implementation for coordinating repository operations.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly ReferenceDbContext _context;
    
    public UnitOfWork(ReferenceDbContext context)
    {
        _context = context;
    }
    
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        await _context.Database.BeginTransactionAsync(cancellationToken);
    }
    
    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await SaveChangesAsync(cancellationToken);
            await _context.Database.CommitTransactionAsync(cancellationToken);
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
    
    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.Database.RollbackTransactionAsync(cancellationToken);
        }
        catch
        {
            // Transaction may have already been rolled back
        }
    }
    
    public void Dispose()
    {
        _context?.Dispose();
    }
    
    public async ValueTask DisposeAsync()
    {
        if (_context != null)
        {
            await _context.DisposeAsync();
        }
    }
}
