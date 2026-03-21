using MediatR;
using TravelRequestService.Domain.Interfaces;

namespace TravelRequestService.Application.Commands;

public class ApproveTravelRequestCommandHandler : IRequestHandler<ApproveTravelRequestCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public ApproveTravelRequestCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(ApproveTravelRequestCommand request, CancellationToken cancellationToken)
    {
        var travelRequest = await _unitOfWork.TravelRequests.GetByIdAsync(
            request.PlanNumber, request.CompanyCode, cancellationToken);

        if (travelRequest is null)
            throw new KeyNotFoundException($"Travel request {request.PlanNumber} not found.");

        travelRequest.Approve(request.ApprovedBy, request.ApprovalAmount, request.Remarks);

        await _unitOfWork.TravelRequests.UpdateAsync(travelRequest, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
