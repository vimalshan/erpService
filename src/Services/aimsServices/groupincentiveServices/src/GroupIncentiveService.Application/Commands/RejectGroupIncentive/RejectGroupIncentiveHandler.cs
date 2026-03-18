using GroupIncentiveService.Domain.Entities;
using GroupIncentiveService.Domain.Exceptions;
using GroupIncentiveService.Domain.Interfaces;
using MediatR;

namespace GroupIncentiveService.Application.Commands.RejectGroupIncentive;

public class RejectGroupIncentiveHandler : IRequestHandler<RejectGroupIncentiveCommand, Unit>
{
    private readonly IGroupIncentiveMainRepository _mainRepo;
    private readonly IGroupIncentiveApprovalRepository _approvalRepo;
    private readonly IUnitOfWork _unitOfWork;

    public RejectGroupIncentiveHandler(IGroupIncentiveMainRepository mainRepo,
        IGroupIncentiveApprovalRepository approvalRepo, IUnitOfWork unitOfWork)
    {
        _mainRepo = mainRepo;
        _approvalRepo = approvalRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(RejectGroupIncentiveCommand request, CancellationToken cancellationToken)
    {
        var incentive = await _mainRepo.GetByIdAsync(request.IncentiveId, cancellationToken)
            ?? throw new NotFoundException(nameof(GroupIncentiveMain), request.IncentiveId);

        incentive.Reject(request.RejectedBy, request.Remarks);

        var approvalId = await _approvalRepo.GetNextIdAsync(cancellationToken);
        var approval = GroupIncentiveApproval.Create(approvalId, incentive.GrpIncId,
            request.RejectedBy, "N", request.Remarks, request.RejectedBy);

        await _approvalRepo.AddAsync(approval, cancellationToken);
        await _mainRepo.UpdateAsync(incentive, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
