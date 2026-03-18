using CompetencyService.Domain.Entities;

namespace CompetencyService.Domain.Interfaces;

public interface ICompetencyRepository
{
    Task<CompetencyMaster?> GetByIdAsync(decimal id, CancellationToken ct = default);
    Task<IEnumerable<CompetencyMaster>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<CompetencyMaster>> GetByTypeAsync(string type, CancellationToken ct = default);
    Task AddAsync(CompetencyMaster entity, CancellationToken ct = default);
    void Update(CompetencyMaster entity);
    void Delete(CompetencyMaster entity);
}

public interface IEmpSpecificCompetencyRepository
{
    Task<IEnumerable<EmpSpecificCompetency>> GetByEmpAsync(decimal empSysId, decimal yearId, CancellationToken ct = default);
    Task AddAsync(EmpSpecificCompetency entity, CancellationToken ct = default);
    void Delete(EmpSpecificCompetency entity);
}

public interface IRoleSpecificRepository
{
    Task<IEnumerable<RoleSpecific>> GetByEmpAsync(decimal empSysId, CancellationToken ct = default);
    Task AddAsync(RoleSpecific entity, CancellationToken ct = default);
    void Update(RoleSpecific entity);
}

public interface ICompetencyRatingScaleRepository
{
    Task<CompetencyRatingScale?> GetByCompetencyIdAsync(decimal competencyId, CancellationToken ct = default);
    Task AddAsync(CompetencyRatingScale entity, CancellationToken ct = default);
    void Update(CompetencyRatingScale entity);
}
