using MasterDataService.Application.Interfaces;
using MasterDataService.Domain.Entities;
using MasterDataService.Domain.Interfaces;
using MasterDataService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MasterDataService.Infrastructure.Repositories;

public class ConfigurationRepository : IConfigurationRepository
{
    private readonly MasterDataDbContext _context;
    public ConfigurationRepository(MasterDataDbContext context) => _context = context;

    public async Task<Configuration?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
        => await _context.Configurations.FindAsync([id], cancellationToken);

    public async Task<Configuration?> GetByIdAsync(int configId, CancellationToken cancellationToken = default)
        => await _context.Configurations.FindAsync([configId], cancellationToken);

    public async Task<IEnumerable<Configuration>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.Configurations.AsNoTracking().ToListAsync(cancellationToken);

    public async Task<Configuration?> GetByKeyAsync(string key, CancellationToken cancellationToken = default)
        => await _context.Configurations.FirstOrDefaultAsync(x => x.ConfigKey == key, cancellationToken);

    public async Task AddAsync(Configuration entity, CancellationToken cancellationToken = default)
        => await _context.Configurations.AddAsync(entity, cancellationToken);

    public void Update(Configuration entity) => _context.Configurations.Update(entity);
    public void Delete(Configuration entity) => _context.Configurations.Remove(entity);
}

public class RateMasterRepository : IRateMasterRepository
{
    private readonly MasterDataDbContext _context;
    public RateMasterRepository(MasterDataDbContext context) => _context = context;

    public async Task<RateMaster?> GetByIdAsync(object id, CancellationToken cancellationToken = default) => null;

    public async Task<RateMaster?> GetByIdAsync(string trustCode, int rateId, CancellationToken cancellationToken = default)
        => await _context.RateMasters.FindAsync([trustCode, rateId], cancellationToken);

    public async Task<IEnumerable<RateMaster>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.RateMasters.AsNoTracking().ToListAsync(cancellationToken);

    public async Task<IEnumerable<RateMaster>> GetByTrustCodeAsync(string trustCode, CancellationToken cancellationToken = default)
        => await _context.RateMasters.Where(x => x.TrustCode == trustCode).AsNoTracking().ToListAsync(cancellationToken);

    public async Task<int> GetNextRateIdAsync(string trustCode, CancellationToken cancellationToken = default)
    {
        var max = await _context.RateMasters.Where(x => x.TrustCode == trustCode).MaxAsync(x => (int?)x.RateId, cancellationToken);
        return (max ?? 0) + 1;
    }

    public async Task AddAsync(RateMaster entity, CancellationToken cancellationToken = default)
        => await _context.RateMasters.AddAsync(entity, cancellationToken);

    public void Update(RateMaster entity) => _context.RateMasters.Update(entity);
    public void Delete(RateMaster entity) => _context.RateMasters.Remove(entity);
}

public class FundTypeRepository : IFundTypeRepository
{
    private readonly MasterDataDbContext _context;
    public FundTypeRepository(MasterDataDbContext context) => _context = context;

    public async Task<FundType?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
        => await _context.FundTypes.FindAsync([id], cancellationToken);

    public async Task<FundType?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
        => await _context.FundTypes.FindAsync([code], cancellationToken);

    public async Task<IEnumerable<FundType>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.FundTypes.AsNoTracking().ToListAsync(cancellationToken);

    public async Task AddAsync(FundType entity, CancellationToken cancellationToken = default)
        => await _context.FundTypes.AddAsync(entity, cancellationToken);

    public void Update(FundType entity) => _context.FundTypes.Update(entity);
    public void Delete(FundType entity) => _context.FundTypes.Remove(entity);
}

public class RoleRepository : IRoleRepository
{
    private readonly MasterDataDbContext _context;
    public RoleRepository(MasterDataDbContext context) => _context = context;

    public async Task<RoleMaster?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
        => await _context.RoleMasters.FindAsync([id], cancellationToken);

    public async Task<RoleMaster?> GetByCodeAsync(long code, CancellationToken cancellationToken = default)
        => await _context.RoleMasters.FindAsync([code], cancellationToken);

    public async Task<IEnumerable<RoleMaster>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.RoleMasters.AsNoTracking().ToListAsync(cancellationToken);

    public async Task<IEnumerable<RoleMaster>> GetActiveRolesAsync(CancellationToken cancellationToken = default)
        => await _context.RoleMasters.Where(x => x.RoleStatus == "A").AsNoTracking().ToListAsync(cancellationToken);

    public async Task AddAsync(RoleMaster entity, CancellationToken cancellationToken = default)
        => await _context.RoleMasters.AddAsync(entity, cancellationToken);

    public void Update(RoleMaster entity) => _context.RoleMasters.Update(entity);
    public void Delete(RoleMaster entity) => _context.RoleMasters.Remove(entity);
}
