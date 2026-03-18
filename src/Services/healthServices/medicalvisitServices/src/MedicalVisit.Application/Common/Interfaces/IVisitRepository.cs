using MedicalVisit.Domain.Entities;

namespace MedicalVisit.Application.Common.Interfaces;

public interface IVisitRepository
{
    Task<VisitMainAggregate?> GetByIdAsync(string companyCode, long visitNumber, CancellationToken cancellationToken = default);
    Task<IEnumerable<VisitMainAggregate>> GetByDateRangeAsync(string companyCode, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    Task<IEnumerable<VisitMainAggregate>> GetByMedicalUserIdAsync(string companyCode, string medicalUserId, CancellationToken cancellationToken = default);
    Task<long> GetNextVisitNumberAsync(string companyCode, CancellationToken cancellationToken = default);
    Task<VisitMainAggregate> AddAsync(VisitMainAggregate visit, CancellationToken cancellationToken = default);
    Task UpdateAsync(VisitMainAggregate visit, CancellationToken cancellationToken = default);
    Task<IEnumerable<VisitMainAggregate>> GetAllAsync(string companyCode, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);
}
