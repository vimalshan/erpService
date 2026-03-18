using Microsoft.EntityFrameworkCore;
using VisitorServices.Application.Common.Interfaces;
using VisitorServices.Domain.Entities;
using VisitorServices.Domain.Enums;
using VisitorServices.Infrastructure.Data;

namespace VisitorServices.Infrastructure.Repositories;

public class ApprovalRequestRepository(VisitorDbContext context) : IApprovalRequestRepository
{
    public async Task<long> GetNextIdAsync(CancellationToken cancellationToken = default)
    {
        var max = await context.ApprovalRequests.MaxAsync(r => (long?)r.Id, cancellationToken) ?? 0L;
        return max + 1;
    }

    public async Task AddAsync(VisitorApprovalRequest request, CancellationToken cancellationToken = default)
        => await context.ApprovalRequests.AddAsync(request, cancellationToken);

    public async Task<VisitorApprovalRequest?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        => await context.ApprovalRequests.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

    public async Task<IEnumerable<VisitorApprovalRequest>> GetPendingByApproverAsync(
        long approverId, CancellationToken cancellationToken = default)
        => await context.ApprovalRequests
            .Where(r => r.RequiredApproverId == approverId && r.ApprovalStatus == ApprovalStatus.Pending)
            .ToListAsync(cancellationToken);

    public void Update(VisitorApprovalRequest request)
        => context.ApprovalRequests.Update(request);
}
