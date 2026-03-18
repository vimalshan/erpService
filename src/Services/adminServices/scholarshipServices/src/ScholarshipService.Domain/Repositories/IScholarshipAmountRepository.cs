using ScholarshipService.Domain.Entities;

namespace ScholarshipService.Domain.Repositories;

public interface IScholarshipAmountRepository
{
    Task<ScholarshipAmount?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ScholarshipAmount>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ScholarshipAmount?> GetEligibleAmountAsync(string gradeCategory, string eligibleExam, int year, CancellationToken cancellationToken = default);
    Task<long> GetNextIdAsync(CancellationToken cancellationToken = default);
    Task AddAsync(ScholarshipAmount amount, CancellationToken cancellationToken = default);
    Task UpdateAsync(ScholarshipAmount amount, CancellationToken cancellationToken = default);
}
