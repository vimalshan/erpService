namespace ObjectiveService.Domain.Interfaces;

/// <summary>Generic domain repository interface usable without referencing EF directly.</summary>
public interface IDomainRepository<T> where T : class
{
    Task<T?> FindByIdAsync(decimal id, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> FindAllAsync(CancellationToken cancellationToken = default);
    Task SaveAsync(T entity, CancellationToken cancellationToken = default);
    Task RemoveAsync(T entity, CancellationToken cancellationToken = default);
}
