using ScholarshipService.Domain.Entities;

namespace ScholarshipService.Domain.Repositories;

public interface IScholarshipDetailRepository
{
    Task<ScholarshipDetail?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ScholarshipDetail>> GetByMainIdAsync(int mainId, CancellationToken cancellationToken = default);
    Task<long> GetNextIdAsync(CancellationToken cancellationToken = default);
    Task AddAsync(ScholarshipDetail detail, CancellationToken cancellationToken = default);
    Task UpdateAsync(ScholarshipDetail detail, CancellationToken cancellationToken = default);
}
