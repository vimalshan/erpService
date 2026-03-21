using LookupService.Domain.Entities;

namespace LookupService.Domain.Interfaces;

public interface ILovMasterRepository
{
    Task<LovMaster?> GetByIdAsync(long lovId, CancellationToken ct = default);
    Task<IEnumerable<LovMaster>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<LovMaster>> GetByTypeAsync(string lovType, CancellationToken ct = default);
    Task AddAsync(LovMaster entity, CancellationToken ct = default);
    void Update(LovMaster entity);
    void Delete(LovMaster entity);
}

public interface ILovTypeMasterRepository
{
    Task<LovTypeMaster?> GetByCodeAsync(string typeCode, CancellationToken ct = default);
    Task<IEnumerable<LovTypeMaster>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(LovTypeMaster entity, CancellationToken ct = default);
    void Update(LovTypeMaster entity);
    void Delete(LovTypeMaster entity);
}

public interface IProcessMasterRepository
{
    Task<ProcessMaster?> GetByIdAsync(decimal processId, CancellationToken ct = default);
    Task<IEnumerable<ProcessMaster>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(ProcessMaster entity, CancellationToken ct = default);
    void Update(ProcessMaster entity);
    void Delete(ProcessMaster entity);
}

public interface IPanelMasterRepository
{
    Task<PanelMaster?> GetByIdAsync(decimal panelId, CancellationToken ct = default);
    Task<IEnumerable<PanelMaster>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(PanelMaster entity, CancellationToken ct = default);
    void Update(PanelMaster entity);
}

public interface IUnitProcessMapRepository
{
    Task<IEnumerable<UnitProcessMap>> GetByUnitCodeAsync(string unitCode, CancellationToken ct = default);
    Task AddAsync(UnitProcessMap entity, CancellationToken ct = default);
    void Delete(UnitProcessMap entity);
}

public interface ILovUnitMapRepository
{
    Task<IEnumerable<LovUnitMap>> GetByLovIdAsync(long lovId, CancellationToken ct = default);
    Task AddAsync(LovUnitMap entity, CancellationToken ct = default);
    void Delete(LovUnitMap entity);
}

public interface IUnitLovAccessMasterRepository
{
    Task<UnitLovAccessMaster?> GetByIdAsync(decimal accessMastId, CancellationToken ct = default);
    Task<IEnumerable<UnitLovAccessMaster>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(UnitLovAccessMaster entity, CancellationToken ct = default);
    void Update(UnitLovAccessMaster entity);
}

public interface IUnitOfWork : IDisposable
{
    ILovMasterRepository LovMasters { get; }
    ILovTypeMasterRepository LovTypeMasters { get; }
    IProcessMasterRepository ProcessMasters { get; }
    IPanelMasterRepository PanelMasters { get; }
    IUnitProcessMapRepository UnitProcessMaps { get; }
    ILovUnitMapRepository LovUnitMaps { get; }
    IUnitLovAccessMasterRepository UnitLovAccessMasters { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
