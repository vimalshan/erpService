using MasterDataService.Domain.Entities;
using MasterDataService.Domain.Interfaces;

namespace MasterDataService.Application.Interfaces;

public interface ILovMasterRepository : IRepository<LovMaster>
{
    Task<LovMaster?> GetByIdAsync(decimal lovId, CancellationToken cancellationToken = default);
    Task<IEnumerable<LovMaster>> GetByCategoryAsync(string category, CancellationToken cancellationToken = default);
    Task<decimal> GetNextIdAsync(CancellationToken cancellationToken = default);
}

public interface IConfigurationRepository : IRepository<Configuration>
{
    Task<Configuration?> GetByIdAsync(int configId, CancellationToken cancellationToken = default);
    Task<Configuration?> GetByKeyAsync(string key, CancellationToken cancellationToken = default);
}

public interface IRateMasterRepository : IRepository<RateMaster>
{
    Task<IEnumerable<RateMaster>> GetByTrustCodeAsync(string trustCode, CancellationToken cancellationToken = default);
    Task<RateMaster?> GetByIdAsync(string trustCode, int rateId, CancellationToken cancellationToken = default);
    Task<int> GetNextRateIdAsync(string trustCode, CancellationToken cancellationToken = default);
}

public interface IFundTypeRepository : IRepository<FundType>
{
    Task<FundType?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
}

public interface IRoleRepository : IRepository<RoleMaster>
{
    Task<RoleMaster?> GetByCodeAsync(long code, CancellationToken cancellationToken = default);
    Task<IEnumerable<RoleMaster>> GetActiveRolesAsync(CancellationToken cancellationToken = default);
}

public interface ICompFinancialYearRepository : IRepository<ComputationFinancialYear>
{
    Task<ComputationFinancialYear?> GetBySerialAsync(long serialNumber, CancellationToken cancellationToken = default);
    Task<ComputationFinancialYear?> GetCurrentYearAsync(CancellationToken cancellationToken = default);
}
