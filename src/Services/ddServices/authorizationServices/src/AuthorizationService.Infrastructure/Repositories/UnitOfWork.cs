using AuthorizationService.Domain.Entities;
using AuthorizationService.Domain.Interfaces;

namespace AuthorizationService.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly Data.AuthorizationDbContext _context;
    private IRepository<Right>? _rightRepository;
    private IRepository<SpecialInput>? _specialInputRepository;
    private IRepository<SpecialInputMaster>? _specialInputMasterRepository;
    private ITrackerRightRepository? _trackerRightRepository;
    private IUserRightRepository? _userRightRepository;

    public UnitOfWork(Data.AuthorizationDbContext context)
    {
        _context = context;
    }

    public IRepository<Right> Rights =>
        _rightRepository ??= new GenericRepository<Right>(_context);

    public IRepository<SpecialInput> SpecialInputs =>
        _specialInputRepository ??= new GenericRepository<SpecialInput>(_context);

    public IRepository<SpecialInputMaster> SpecialInputMasters =>
        _specialInputMasterRepository ??= new GenericRepository<SpecialInputMaster>(_context);

    public ITrackerRightRepository TrackerRights =>
        _trackerRightRepository ??= new TrackerRightRepository(_context);

    public IUserRightRepository UserRights =>
        _userRightRepository ??= new UserRightRepository(_context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            await _context.Database.CommitTransactionAsync(cancellationToken);
        }
        catch
        {
            await RollbackAsync();
            throw;
        }
    }

    public async Task RollbackAsync()
    {
        try
        {
            await _context.Database.RollbackTransactionAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error rolling back transaction: {ex.Message}");
        }
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}
