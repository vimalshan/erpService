using FaqServices.Domain.Entities;

namespace FaqServices.Domain.Interfaces;

public interface IFaqGradeRepository
{
    Task<FaqGrade?> GetByIdAsync(string id, CancellationToken ct = default);
    Task<IEnumerable<FaqGrade>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<FaqGrade>> GetActiveAsync(CancellationToken ct = default);
    Task AddAsync(FaqGrade grade, CancellationToken ct = default);
    void Update(FaqGrade grade);
    void Remove(FaqGrade grade);
    Task<bool> ExistsAsync(string id, CancellationToken ct = default);
}
