using VisitorServices.Domain.Aggregates;

namespace VisitorServices.Application.Common.Interfaces;

public interface IVisitorRepository
{
    Task<VisitorAggregate?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<VisitorAggregate>> GetActiveVisitorsAsync(CancellationToken cancellationToken = default);
    Task<long> GetNextIdAsync(CancellationToken cancellationToken = default);
    Task AddAsync(VisitorAggregate visitor, CancellationToken cancellationToken = default);
    void Update(VisitorAggregate visitor);
}
