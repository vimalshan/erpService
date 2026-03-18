namespace AccessService.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore.Storage;
using AccessService.Infrastructure.Persistence;
using AppIUoW = AccessService.Application.Interfaces.IUnitOfWork;

/// <summary>
/// Unit of Work implementation for coordinating repository operations
/// </summary>
public class UnitOfWork : IUnitOfWork, AppIUoW
{
    private readonly AccessServiceDbContext _context;
    private IDbContextTransaction? _transaction;
    
    private IUserMapRepository? _userMapRepository;
    private IUserRoleRepository? _userRoleRepository;
    private IMenuRepository? _menuRepository;
    private ISPARSHMenuRepository? _sparshMenuRepository;
    private ISPARSHMenuAccessRepository? _sparshMenuAccessRepository;

    public UnitOfWork(AccessServiceDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public IUserMapRepository UserMaps
    {
        get { return _userMapRepository ??= new EFUserMapRepository(_context); }
    }

    public IUserRoleRepository UserRoles
    {
        get { return _userRoleRepository ??= new EFUserRoleRepository(_context); }
    }

    public IMenuRepository Menus
    {
        get { return _menuRepository ??= new EFMenuRepository(_context); }
    }

    public ISPARSHMenuRepository SPARSHMenus
    {
        get { return _sparshMenuRepository ??= new EFSPARSHMenuRepository(_context); }
    }

    public ISPARSHMenuAccessRepository SPARSHMenuAccess
    {
        get { return _sparshMenuAccessRepository ??= new EFSPARSHMenuAccessRepository(_context); }
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
        return true;
    }

    public async Task<bool> CommitTransactionAsync()
    {
        try
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                return true;
            }
            return false;
        }
        finally
        {
            _transaction?.Dispose();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        try
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
            }
        }
        finally
        {
            _transaction?.Dispose();
            _transaction = null;
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_transaction != null)
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        await _context.DisposeAsync();
    }

    // Explicit implementations for Application.Interfaces.IUnitOfWork
    dynamic AppIUoW.UserMaps => UserMaps;
    dynamic AppIUoW.UserRoles => UserRoles;
    dynamic AppIUoW.Menus => Menus;
    dynamic AppIUoW.SPARSHMenus => SPARSHMenus;
    dynamic AppIUoW.SPARSHMenuAccesses => SPARSHMenuAccess;

    async Task AppIUoW.BeginTransactionAsync(CancellationToken cancellationToken)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    async Task AppIUoW.CommitTransactionAsync(CancellationToken cancellationToken)
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    async Task AppIUoW.RollbackTransactionAsync(CancellationToken cancellationToken)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    void IDisposable.Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
