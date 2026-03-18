using OrganizationSetup.Application.Interfaces;
using OrganizationSetup.Infrastructure.Persistence;
using OrganizationSetup.Infrastructure.Repositories;

namespace OrganizationSetup.Infrastructure.Services;

public class UnitOfWork : IUnitOfWork
{
    private readonly OrganizationSetupDbContext _context;
    private IRoleRepository? _roleRepository;
    private IUserMapRepository? _userMapRepository;
    private IOrgParamsRepository? _orgParamsRepository;
    private IPpLimitRepository? _ppLimitRepository;

    public UnitOfWork(OrganizationSetupDbContext context) => _context = context;

    public IRoleRepository Roles => _roleRepository ??= new RoleRepository(_context);
    public IUserMapRepository UserMaps => _userMapRepository ??= new UserMapRepository(_context);
    public IOrgParamsRepository OrgParams => _orgParamsRepository ??= new OrgParamsRepository(_context);
    public IPpLimitRepository PpLimits => _ppLimitRepository ??= new PpLimitRepository(_context);

    public async Task<int> SaveChangesAsync(CancellationToken ct = default) =>
        await _context.SaveChangesAsync(ct);

    public async Task BeginTransactionAsync(CancellationToken ct = default) =>
        await _context.Database.BeginTransactionAsync(ct);

    public async Task CommitAsync(CancellationToken ct = default) =>
        await _context.Database.CommitTransactionAsync(CancellationToken.None);

    public async Task RollbackAsync(CancellationToken ct = default) =>
        await _context.Database.RollbackTransactionAsync(CancellationToken.None);

    public void Dispose() => _context.Dispose();
}
