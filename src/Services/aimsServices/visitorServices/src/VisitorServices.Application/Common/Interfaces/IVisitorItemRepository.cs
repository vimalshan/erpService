using VisitorServices.Domain.Entities;

namespace VisitorServices.Application.Common.Interfaces;

public interface IVisitorItemRepository
{
    Task<long> GetNextIdAsync(CancellationToken cancellationToken = default);
    Task AddAsync(VisitorItem item, CancellationToken cancellationToken = default);
    Task<IEnumerable<VisitorItem>> GetByVisitorIdAsync(long visitorId, CancellationToken cancellationToken = default);
}
