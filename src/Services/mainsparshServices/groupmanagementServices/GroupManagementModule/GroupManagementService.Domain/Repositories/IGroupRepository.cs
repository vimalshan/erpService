namespace GroupManagementService.Domain.Repositories
{
    /// <summary>
    /// Repository interface for Group entity
    /// </summary>
    public interface IGroupRepository
    {
        Task<Entities.Group?> GetByIdAsync(long groupId, CancellationToken cancellationToken = default);
        Task<Entities.Group?> GetByCodeAsync(string groupCode, CancellationToken cancellationToken = default);
        Task<IEnumerable<Entities.Group>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<Entities.Group>> GetByStatusAsync(ValueObjects.GroupStatus status, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(long groupId, CancellationToken cancellationToken = default);
        Task<bool> CodeExistsAsync(string groupCode, CancellationToken cancellationToken = default);
        Task AddAsync(Entities.Group group, CancellationToken cancellationToken = default);
        Task UpdateAsync(Entities.Group group, CancellationToken cancellationToken = default);
        Task DeleteAsync(long groupId, CancellationToken cancellationToken = default);
    }
}
