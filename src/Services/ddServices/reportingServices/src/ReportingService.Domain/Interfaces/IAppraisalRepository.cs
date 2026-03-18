namespace ReportingService.Domain.Interfaces;

/// <summary>
/// Repository interface for Appraisal entity
/// </summary>
public interface IAppraisalRepository
{
    Task<Entities.Appraisal?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<Entities.Appraisal?> GetByRequestNumberAsync(long requestNumber, CancellationToken cancellationToken = default);
    Task<IEnumerable<Entities.Appraisal>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Entities.Appraisal>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task AddAsync(Entities.Appraisal entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(Entities.Appraisal entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}
