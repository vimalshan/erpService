using CompensationBenefits.Domain.Entities;

namespace CompensationBenefits.Domain.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(T entity, CancellationToken ct = default);
    void Update(T entity);
    void Remove(T entity);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}

public interface ISalaryRepository : IRepository<SalaryMain>
{
    Task<SalaryMain?> GetWithDetailsAsync(long salaryId, CancellationToken ct = default);
    Task<IEnumerable<SalaryMain>> GetByStructureIdAsync(long structureId, CancellationToken ct = default);
}

public interface ISalaryStructureRepository : IRepository<SalaryStructureMain>
{
    Task<SalaryStructureMain?> GetWithDetailsAsync(long structureId, CancellationToken ct = default);
    Task<IEnumerable<SalaryStructureMain>> GetByUnitIdAsync(long unitId, CancellationToken ct = default);
}

public interface IMediclaimRepository : IRepository<MediclaimMaster>
{
    Task<MediclaimMaster?> GetWithDetailsAsync(long mediclaimId, CancellationToken ct = default);
}

public interface IMobileConnectionRepository : IRepository<MobileConnection>
{
    Task<IEnumerable<MobileConnection>> GetByEmployeeAsync(long empSysId, CancellationToken ct = default);
}

public interface IRetiralRangeMasterRepository : IRepository<RetiralRangeMaster>
{
    Task<IEnumerable<RetiralRangeMaster>> GetByUnitIdAsync(long unitId, CancellationToken ct = default);
}
