using Microsoft.EntityFrameworkCore;
using ReferenceDataService.Domain.Entities;
using ReferenceDataService.Domain.Interfaces;
using ReferenceDataService.Infrastructure.Persistence;

namespace ReferenceDataService.Infrastructure.Repositories;

public class PathToSqlServerRepository : IPathToSqlServerRepository
{
    private readonly ReferenceDataDbContext _context;

    public PathToSqlServerRepository(ReferenceDataDbContext context)
    {
        _context = context;
    }

    public async Task<PathToSqlServer?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.PathToSqlServers.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IEnumerable<PathToSqlServer>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.PathToSqlServers.ToListAsync(cancellationToken);
    }

    public async Task<PathToSqlServer?> GetByCompanyCodeAsync(string companyCode, CancellationToken cancellationToken = default)
    {
        return await _context.PathToSqlServers
            .FirstOrDefaultAsync(x => x.CompanyCode == companyCode, cancellationToken);
    }

    public async Task AddAsync(PathToSqlServer entity, CancellationToken cancellationToken = default)
    {
        await _context.PathToSqlServers.AddAsync(entity, cancellationToken);
    }

    public void Update(PathToSqlServer entity)
    {
        _context.PathToSqlServers.Update(entity);
    }

    public void Delete(PathToSqlServer entity)
    {
        _context.PathToSqlServers.Remove(entity);
    }
}
