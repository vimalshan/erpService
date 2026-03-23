using ReferenceService.Domain.Entities;

namespace ReferenceService.Domain.Interfaces;

/// <summary>
/// Repository interface for LovValue entity.
/// </summary>
public interface ILovValueRepository : IRepository<LovValue, int>
{
    /// <summary>
    /// Get LovValue by code asynchronously.
    /// </summary>
    Task<LovValue?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all LovValues for a specific type, ordered by sequence asynchronously.
    /// </summary>
    Task<List<LovValue>> GetByTypeIdAsync(int typeId, CancellationToken cancellationToken = default);
}
