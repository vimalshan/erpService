using VisitorServices.Domain.Entities;

namespace VisitorServices.Application.Common.Interfaces;

public interface IApprovalRequestRepository
{
    Task<long> GetNextIdAsync(CancellationToken cancellationToken = default);
    Task AddAsync(VisitorApprovalRequest request, CancellationToken cancellationToken = default);
    Task<VisitorApprovalRequest?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<VisitorApprovalRequest>> GetPendingByApproverAsync(long approverId, CancellationToken cancellationToken = default);
    void Update(VisitorApprovalRequest request);
}
