using HealthTransaction.Domain.Entities;

namespace HealthTransaction.Domain.Interfaces;

public interface IPreEmploymentCheckupRepository
{
    Task<PreEmploymentCheckup?> GetByKeyAsync(decimal empNum, string comCode, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PreEmploymentCheckup>> GetByEmployeeNumAsync(decimal empNum, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PreEmploymentCheckup>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PreEmploymentCheckup>> GetByDateRangeAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default);
    Task AddAsync(PreEmploymentCheckup entity, CancellationToken cancellationToken = default);
    void Update(PreEmploymentCheckup entity);
    void Remove(PreEmploymentCheckup entity);
}
