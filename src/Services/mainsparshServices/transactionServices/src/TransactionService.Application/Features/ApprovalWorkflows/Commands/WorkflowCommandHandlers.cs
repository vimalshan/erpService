using AutoMapper;
using MediatR;
using TransactionService.Application.DTOs;
using TransactionService.Domain.Entities;
using TransactionService.Domain.Exceptions;
using TransactionService.Domain.Interfaces;

namespace TransactionService.Application.Features.ApprovalWorkflows.Commands;

public class SubmitWorkflowCommandHandler : IRequestHandler<SubmitWorkflowCommand, ApprovalWorkflowDto>
{
    private readonly IApprovalWorkflowRepository _repository;
    private readonly IMapper _mapper;

    public SubmitWorkflowCommandHandler(IApprovalWorkflowRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ApprovalWorkflowDto> Handle(SubmitWorkflowCommand request, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetByEntityAsync(request.EntityType, request.EntityId, cancellationToken);
        if (existing.Any(w => w.WorkflowStatus is "SUBMITTED" or "IN_REVIEW"))
            throw new DomainException($"An active workflow already exists for {request.EntityType}:{request.EntityId}.");

        var workflow = ApprovalWorkflow.Create(
            request.EntityType,
            request.EntityId,
            request.EmployeeId,
            request.CurrentApproverId,
            request.MaxApprovalLevels,
            request.Remarks,
            request.CreatedBy);

        await _repository.AddAsync(workflow, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return _mapper.Map<ApprovalWorkflowDto>(workflow);
    }
}

public class ApproveStepCommandHandler : IRequestHandler<ApproveStepCommand, ApprovalWorkflowDto>
{
    private readonly IApprovalWorkflowRepository _repository;
    private readonly IMapper _mapper;

    public ApproveStepCommandHandler(IApprovalWorkflowRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ApprovalWorkflowDto> Handle(ApproveStepCommand request, CancellationToken cancellationToken)
    {
        var workflow = await _repository.GetByIdAsync(request.WorkflowId, cancellationToken)
            ?? throw new KeyNotFoundException($"Workflow {request.WorkflowId} not found.");

        workflow.ApproveCurrentStep(request.ApproverId, request.Remarks);

        if (request.NextApproverId.HasValue && workflow.WorkflowStatus == "IN_REVIEW")
            workflow.AddNextStep(request.NextApproverId.Value, request.ApproverId);

        _repository.Update(workflow);
        await _repository.SaveChangesAsync(cancellationToken);
        return _mapper.Map<ApprovalWorkflowDto>(workflow);
    }
}

public class RejectStepCommandHandler : IRequestHandler<RejectStepCommand, ApprovalWorkflowDto>
{
    private readonly IApprovalWorkflowRepository _repository;
    private readonly IMapper _mapper;

    public RejectStepCommandHandler(IApprovalWorkflowRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ApprovalWorkflowDto> Handle(RejectStepCommand request, CancellationToken cancellationToken)
    {
        var workflow = await _repository.GetByIdAsync(request.WorkflowId, cancellationToken)
            ?? throw new KeyNotFoundException($"Workflow {request.WorkflowId} not found.");

        workflow.RejectCurrentStep(request.ApproverId, request.Remarks);

        _repository.Update(workflow);
        await _repository.SaveChangesAsync(cancellationToken);
        return _mapper.Map<ApprovalWorkflowDto>(workflow);
    }
}

public class CancelWorkflowCommandHandler : IRequestHandler<CancelWorkflowCommand, bool>
{
    private readonly IApprovalWorkflowRepository _repository;

    public CancelWorkflowCommandHandler(IApprovalWorkflowRepository repository) => _repository = repository;

    public async Task<bool> Handle(CancelWorkflowCommand request, CancellationToken cancellationToken)
    {
        var workflow = await _repository.GetByIdAsync(request.WorkflowId, cancellationToken)
            ?? throw new KeyNotFoundException($"Workflow {request.WorkflowId} not found.");

        workflow.Cancel(request.CancelledBy, request.Remarks);

        _repository.Update(workflow);
        await _repository.SaveChangesAsync(cancellationToken);
        return true;
    }
}
