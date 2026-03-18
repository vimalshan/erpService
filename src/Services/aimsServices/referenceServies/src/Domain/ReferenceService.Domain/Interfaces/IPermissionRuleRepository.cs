using ReferenceService.Domain.Entities;

namespace ReferenceService.Domain.Interfaces;

/// <summary>
/// Repository interface for PermissionRule aggregate root.
/// </summary>
public interface IPermissionRuleRepository : IRepository<PermissionRule, int>
{
    /// <summary>
    /// Get PermissionRule by resource and action asynchronously.
    /// </summary>
    Task<PermissionRule?> GetByResourceAndActionAsync(string resourceId, string action, CancellationToken cancellationToken = default);
}
