using MediatR;
using SSCTransactional.Application.DTOs;
using SSCTransactional.Domain.Entities;
using SSCTransactional.Domain.Exceptions;
using SSCTransactional.Domain.Interfaces;

namespace SSCTransactional.Application.Commands.Approval;

public class CreateApprovalCommandHandler : IRequestHandler<CreateApprovalCommand, DocumentApprovalDto>
{
    private readonly IDocumentApprovalRepository _repo;
    private readonly IUnitOfWork _unitOfWork;

    public CreateApprovalCommandHandler(IDocumentApprovalRepository repo, IUnitOfWork unitOfWork)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
    }

    public async Task<DocumentApprovalDto> Handle(CreateApprovalCommand cmd, CancellationToken ct)
    {
        var id = await _repo.GetNextIdAsync(ct);
        var approval = DocumentApproval.Create(id, cmd.DocId, cmd.ApproverUserId, cmd.Status, cmd.ApprovalDate, cmd.Remarks);
        await _repo.AddAsync(approval, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return new DocumentApprovalDto(approval.Id, approval.DocId, approval.ApproverUserId,
            approval.Status, approval.Remarks, approval.ApprovalDate);
    }
}

public class UpdateApprovalStatusCommandHandler : IRequestHandler<UpdateApprovalStatusCommand, DocumentApprovalDto>
{
    private readonly IDocumentApprovalRepository _repo;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateApprovalStatusCommandHandler(IDocumentApprovalRepository repo, IUnitOfWork unitOfWork)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
    }

    public async Task<DocumentApprovalDto> Handle(UpdateApprovalStatusCommand cmd, CancellationToken ct)
    {
        var approval = await _repo.GetByIdAsync(cmd.ApprovalId, ct)
            ?? throw new ApprovalNotFoundException(cmd.ApprovalId);
        approval.UpdateStatus(cmd.Status, cmd.Remarks);
        await _repo.UpdateAsync(approval, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return new DocumentApprovalDto(approval.Id, approval.DocId, approval.ApproverUserId,
            approval.Status, approval.Remarks, approval.ApprovalDate);
    }
}
