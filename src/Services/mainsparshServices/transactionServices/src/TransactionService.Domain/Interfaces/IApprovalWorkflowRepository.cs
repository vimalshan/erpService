using TransactionService.Domain.Entities;

namespace TransactionService.Domain.Interfaces;

public interface IApprovalWorkflowRepository
{
    Task<ApprovalWorkflow?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ApprovalWorkflow?> GetByCodeAsync(string workflowCode, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ApprovalWorkflow>> GetPendingByApproverAsync(long approverId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ApprovalWorkflow>> GetByEntityAsync(string entityType, long entityId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ApprovalWorkflow>> GetAllAsync(CancellationToken cancellationToken = default);
    IQueryable<ApprovalWorkflow> GetQueryable();
    Task AddAsync(ApprovalWorkflow workflow, CancellationToken cancellationToken = default);
    void Update(ApprovalWorkflow workflow);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
