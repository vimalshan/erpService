using MediatR;
using TransactionService.Application.DTOs;

namespace TransactionService.Application.Features.ApprovalWorkflows.Queries;

public record GetWorkflowByIdQuery(long WorkflowId) : IRequest<ApprovalWorkflowDto?>;
public record GetWorkflowByCodeQuery(string WorkflowCode) : IRequest<ApprovalWorkflowDto?>;
public record GetPendingWorkflowsQuery(long ApproverId) : IRequest<IEnumerable<ApprovalWorkflowDto>>;
public record GetWorkflowsByEntityQuery(string EntityType, long EntityId) : IRequest<IEnumerable<ApprovalWorkflowDto>>;
public record GetAllWorkflowsQuery() : IRequest<IEnumerable<ApprovalWorkflowDto>>;
