using TransactionService.Application.DTOs;
using TransactionService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace TransactionService.API.GraphQL;

public class TransactionQuery
{
    [UseFiltering]
    [UseSorting]
    public IQueryable<ApprovalWorkflowDto> GetApprovalWorkflows([Service] TransactionDbContext context) =>
        context.ApprovalWorkflows
            .Include(w => w.Steps)
            .AsNoTracking()
            .Select(w => new ApprovalWorkflowDto(
                w.Id,
                w.WorkflowCode,
                w.EntityType,
                w.EntityId,
                w.EmployeeId,
                w.WorkflowStatus,
                w.CurrentApprovalLevel,
                w.CurrentApproverId,
                w.MaxApprovalLevels,
                w.Remarks,
                w.CreatedBy,
                w.CreatedOn,
                w.UpdatedBy,
                w.UpdatedOn,
                w.Steps.Select(s => new ApprovalStepDto(
                    s.Id,
                    s.WorkflowId,
                    s.StepLevel,
                    s.ApproverId,
                    s.StepStatus,
                    s.StepRemarks,
                    s.ActedOn,
                    s.CreatedBy,
                    s.CreatedOn)).ToList()));

    [UseFiltering]
    [UseSorting]
    public IQueryable<TransactionLogDto> GetTransactionLogs([Service] TransactionDbContext context) =>
        context.TransactionLogs
            .AsNoTracking()
            .Select(l => new TransactionLogDto(
                l.Id,
                l.TransactionType,
                l.TransactionId,
                l.Action,
                l.ActionBy,
                l.ActionData,
                l.PreviousStatus,
                l.NewStatus,
                l.IpAddress,
                l.CreatedOn));
}
