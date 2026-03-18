using CompensationService.Domain.Entities;

namespace CompensationService.Domain.Repositories;

/// <summary>
/// Repository interface for CompensationGrade aggregate
/// </summary>
public interface ICompensationGradeRepository
{
    Task<CompensationGrade?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<CompensationGrade?> GetByCodeAsync(string gradeCode, CancellationToken cancellationToken = default);
    Task<IEnumerable<CompensationGrade>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<CompensationGrade>> GetActiveAsync(CancellationToken cancellationToken = default);
    Task AddAsync(CompensationGrade grade, CancellationToken cancellationToken = default);
    Task UpdateAsync(CompensationGrade grade, CancellationToken cancellationToken = default);
    Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}
