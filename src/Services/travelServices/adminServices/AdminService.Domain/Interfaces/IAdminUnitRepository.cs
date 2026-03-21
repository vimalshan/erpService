using AdminService.Domain.Entities;

namespace AdminService.Domain.Interfaces;

/// <summary>
/// Repository interface for AdminUnit entity
/// </summary>
public interface IAdminUnitRepository
{
    Task<AdminUnit?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<AdminUnit?> GetByAdminCodeAsync(long adminCode, CancellationToken cancellationToken = default);
    Task<IEnumerable<AdminUnit>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<AdminUnit>> GetByTypeAsync(string adminType, CancellationToken cancellationToken = default);
    Task<AdminUnit> AddAsync(AdminUnit adminUnit, CancellationToken cancellationToken = default);
    Task<AdminUnit> UpdateAsync(AdminUnit adminUnit, CancellationToken cancellationToken = default);
    Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}
