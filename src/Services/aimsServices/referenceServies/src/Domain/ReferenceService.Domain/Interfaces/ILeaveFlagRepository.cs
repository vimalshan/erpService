using ReferenceService.Domain.Entities;

namespace ReferenceService.Domain.Interfaces;

/// <summary>
/// Repository interface for LeaveFlag aggregate root.
/// </summary>
public interface ILeaveFlagRepository : IRepository<LeaveFlag, int>
{
    /// <summary>
    /// Get LeaveFlag by code asynchronously.
    /// </summary>
    Task<LeaveFlag?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
}
