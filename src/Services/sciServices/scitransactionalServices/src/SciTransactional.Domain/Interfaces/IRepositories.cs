using SciTransactional.Domain.Entities;

namespace SciTransactional.Domain.Interfaces;

public interface INavigationRepository
{
    Task<SparshNavigationEntity?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IReadOnlyList<SparshNavigationEntity>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<SparshNavigationEntity>> GetByUserIdAsync(string userId, CancellationToken ct = default);
    Task AddAsync(SparshNavigationEntity entity, CancellationToken ct = default);
    void Update(SparshNavigationEntity entity);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}

public interface INormsRepository
{
    Task<NormsMainEntity?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IReadOnlyList<NormsMainEntity>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<NormsMasterEntity>> GetDetailsByNormNoAsync(long normNo, CancellationToken ct = default);
    Task AddAsync(NormsMainEntity entity, CancellationToken ct = default);
    Task AddDetailAsync(NormsMasterEntity entity, CancellationToken ct = default);
    void Update(NormsMainEntity entity);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}

public interface IAdvanceLicenseRepository
{
    Task<AdvanceLicenseEntity?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IReadOnlyList<AdvanceLicenseEntity>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(AdvanceLicenseEntity entity, CancellationToken ct = default);
    void Update(AdvanceLicenseEntity entity);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}

public interface IAutoMailRepository
{
    Task<IReadOnlyList<AutoMailStatusEntity>> GetAllStatusAsync(CancellationToken ct = default);
    Task<IReadOnlyList<AutoMailIdEntity>> GetAllMailIdsAsync(CancellationToken ct = default);
    Task AddStatusAsync(AutoMailStatusEntity entity, CancellationToken ct = default);
    Task AddMailIdAsync(AutoMailIdEntity entity, CancellationToken ct = default);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}

public interface IOrderMapRepository
{
    Task<IReadOnlyList<ActualOrderMapEntity>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<ActualOrderMapEntity>> GetByTiedOrderIdAsync(decimal tiedOrderDetailId, CancellationToken ct = default);
    Task AddAsync(ActualOrderMapEntity entity, CancellationToken ct = default);
    void Update(ActualOrderMapEntity entity);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}

public interface IDirectEntryRepository
{
    Task<IReadOnlyList<VehicleDirectEntryEntity>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<VehicleDirectEntryEntity>> GetByTrackingNumberAsync(long trackingNumber, CancellationToken ct = default);
    Task AddAsync(VehicleDirectEntryEntity entity, CancellationToken ct = default);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
