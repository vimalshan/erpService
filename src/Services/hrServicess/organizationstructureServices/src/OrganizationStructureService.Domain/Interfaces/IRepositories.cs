using OrganizationStructureService.Domain.Entities;

namespace OrganizationStructureService.Domain.Interfaces;

public interface IBusinessRepository
{
    Task<Business?> GetByIdAsync(decimal businessId, CancellationToken ct = default);
    Task<IReadOnlyList<Business>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Business>> GetActiveAsync(CancellationToken ct = default);
    Task AddAsync(Business business, CancellationToken ct = default);
    Task UpdateAsync(Business business, CancellationToken ct = default);
    Task<bool> ExistsAsync(decimal businessId, CancellationToken ct = default);
}

public interface IUnitRepository
{
    Task<Unit?> GetByIdAsync(decimal unitId, CancellationToken ct = default);
    Task<IReadOnlyList<Unit>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Unit>> GetByBusinessIdAsync(decimal businessId, CancellationToken ct = default);
    Task<IReadOnlyList<Unit>> GetActiveAsync(CancellationToken ct = default);
    Task AddAsync(Unit unit, CancellationToken ct = default);
    Task UpdateAsync(Unit unit, CancellationToken ct = default);
    Task<bool> ExistsAsync(decimal unitId, CancellationToken ct = default);
}

public interface IDepartmentRepository
{
    Task<Department?> GetByIdAsync(decimal departmentId, CancellationToken ct = default);
    Task<IReadOnlyList<Department>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(Department department, CancellationToken ct = default);
    Task UpdateAsync(Department department, CancellationToken ct = default);
}

public interface IDivisionRepository
{
    Task<Division?> GetByIdAsync(decimal divisionId, CancellationToken ct = default);
    Task<IReadOnlyList<Division>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(Division division, CancellationToken ct = default);
    Task UpdateAsync(Division division, CancellationToken ct = default);
}

public interface IGradeRepository
{
    Task<Grade?> GetByIdAsync(decimal gradeId, CancellationToken ct = default);
    Task<IReadOnlyList<Grade>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Grade>> GetActiveAsync(CancellationToken ct = default);
    Task AddAsync(Grade grade, CancellationToken ct = default);
    Task UpdateAsync(Grade grade, CancellationToken ct = default);
}

public interface IPositionRepository
{
    Task<Position?> GetByIdAsync(decimal positionId, CancellationToken ct = default);
    Task<IReadOnlyList<Position>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Position>> GetByUnitCodeAsync(string unitCode, CancellationToken ct = default);
    Task AddAsync(Position position, CancellationToken ct = default);
    Task UpdateAsync(Position position, CancellationToken ct = default);
}

public interface ISiteRepository
{
    Task<Site?> GetByIdAsync(decimal siteId, CancellationToken ct = default);
    Task<IReadOnlyList<Site>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(Site site, CancellationToken ct = default);
    Task UpdateAsync(Site site, CancellationToken ct = default);
}
