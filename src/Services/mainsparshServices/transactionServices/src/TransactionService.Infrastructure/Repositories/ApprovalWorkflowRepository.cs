using Microsoft.EntityFrameworkCore;
using TransactionService.Domain.Entities;
using TransactionService.Domain.Interfaces;
using TransactionService.Infrastructure.Persistence;

namespace TransactionService.Infrastructure.Repositories;

public class ApprovalWorkflowRepository : IApprovalWorkflowRepository
{
    private readonly TransactionDbContext _context;

    public ApprovalWorkflowRepository(TransactionDbContext context) => _context = context;

    public async Task<ApprovalWorkflow?> GetByIdAsync(long id, CancellationToken cancellationToken = default) =>
        await _context.ApprovalWorkflows
            .Include(w => w.Steps)
            .FirstOrDefaultAsync(w => w.Id == id, cancellationToken);

    public async Task<ApprovalWorkflow?> GetByCodeAsync(string workflowCode, CancellationToken cancellationToken = default) =>
        await _context.ApprovalWorkflows
            .Include(w => w.Steps)
            .FirstOrDefaultAsync(w => w.WorkflowCode == workflowCode, cancellationToken);

    public async Task<IReadOnlyList<ApprovalWorkflow>> GetPendingByApproverAsync(long approverId, CancellationToken cancellationToken = default) =>
        await _context.ApprovalWorkflows
            .Include(w => w.Steps)
            .Where(w => w.CurrentApproverId == approverId && (w.WorkflowStatus == "SUBMITTED" || w.WorkflowStatus == "IN_REVIEW"))
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<ApprovalWorkflow>> GetByEntityAsync(string entityType, long entityId, CancellationToken cancellationToken = default) =>
        await _context.ApprovalWorkflows
            .Include(w => w.Steps)
            .Where(w => w.EntityType == entityType && w.EntityId == entityId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<ApprovalWorkflow>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await _context.ApprovalWorkflows
            .Include(w => w.Steps)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public IQueryable<ApprovalWorkflow> GetQueryable() =>
        _context.ApprovalWorkflows.Include(w => w.Steps).AsNoTracking();

    public async Task AddAsync(ApprovalWorkflow workflow, CancellationToken cancellationToken = default) =>
        await _context.ApprovalWorkflows.AddAsync(workflow, cancellationToken);

    public void Update(ApprovalWorkflow workflow) =>
        _context.ApprovalWorkflows.Update(workflow);

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await _context.SaveChangesAsync(cancellationToken);
}
