using ReferenceService.Domain.Entities;

namespace ReferenceService.Domain.Interfaces;

/// <summary>
/// Repository interface for LovType aggregate root.
/// </summary>
public interface ILovTypeRepository : IRepository<LovType, int>
{
    /// <summary>
    /// Get LovType by name asynchronously.
    /// </summary>
    Task<LovType?> GetByNameAsync(string typeName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all LovTypes with their related LovValues asynchronously.
    /// </summary>
    Task<List<LovType>> GetAllWithValuesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get a specific LovType with all related LovValues asynchronously.
    /// </summary>
    Task<LovType?> GetWithValuesAsync(int id, CancellationToken cancellationToken = default);
}
