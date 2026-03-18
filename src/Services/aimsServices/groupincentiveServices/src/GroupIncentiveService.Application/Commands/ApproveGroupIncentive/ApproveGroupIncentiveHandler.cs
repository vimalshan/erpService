using GroupIncentiveService.Domain.Entities;
using GroupIncentiveService.Domain.Exceptions;
using GroupIncentiveService.Domain.Interfaces;
using MediatR;

namespace GroupIncentiveService.Application.Commands.ApproveGroupIncentive;

public class ApproveGroupIncentiveHandler : IRequestHandler<ApproveGroupIncentiveCommand, Unit>
{
    private readonly IGroupIncentiveMainRepository _mainRepo;
    private readonly IGroupIncentiveApprovalRepository _approvalRepo;
    private readonly IUnitOfWork _unitOfWork;

    public ApproveGroupIncentiveHandler(IGroupIncentiveMainRepository mainRepo,
        IGroupIncentiveApprovalRepository approvalRepo, IUnitOfWork unitOfWork)
    {
        _mainRepo = mainRepo;
        _approvalRepo = approvalRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(ApproveGroupIncentiveCommand request, CancellationToken cancellationToken)
    {
        var incentive = await _mainRepo.GetByIdAsync(request.IncentiveId, cancellationToken)
            ?? throw new NotFoundException(nameof(GroupIncentiveMain), request.IncentiveId);

        incentive.Approve(request.ApprovedAmount, request.ApprovedBy);

        var approvalId = await _approvalRepo.GetNextIdAsync(cancellationToken);
        var approval = GroupIncentiveApproval.Create(approvalId, incentive.GrpIncId,
            request.ApprovedBy, "Y", null, request.ApprovedBy);

        await _approvalRepo.AddAsync(approval, cancellationToken);
        await _mainRepo.UpdateAsync(incentive, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
