using MediatR;
using Stationery.Application.DTOs;
using Stationery.Domain.Entities;
using Stationery.Domain.Events;
using Stationery.Domain.Interfaces;
using MassTransit;

namespace Stationery.Application.Features.Requests.Commands;

public record ApproveRequestCommand(
    long RequestSubId,
    long ApprovedQty,
    long ApproverSysId,
    string? Remarks = null
) : IRequest<Unit>;

public class ApproveRequestCommandHandler : IRequestHandler<ApproveRequestCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublishEndpoint _publishEndpoint;

    public ApproveRequestCommandHandler(IUnitOfWork unitOfWork, IPublishEndpoint publishEndpoint)
    {
        _unitOfWork = unitOfWork;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<Unit> Handle(ApproveRequestCommand request, CancellationToken cancellationToken)
    {
        var requestSub = await _unitOfWork.Repository<RequestSub>().GetByIdAsync(request.RequestSubId)
            ?? throw new KeyNotFoundException($"RequestSub {request.RequestSubId} not found.");

        if (requestSub.Status != "P" && requestSub.Status != "I")
            throw new InvalidOperationException($"RequestSub {request.RequestSubId} is not in a pending/indented state.");

        requestSub.ApprovedQty = request.ApprovedQty;
        requestSub.ApproverSysId = request.ApproverSysId;
        requestSub.ApproverRemarks = request.Remarks;
        requestSub.Status = "A";
        requestSub.ApprovedOn = DateTime.UtcNow;
        requestSub.UpdatedBy = request.ApproverSysId;
        requestSub.UpdatedOn = DateTime.UtcNow;

        requestSub.AddDomainEvent(new RequestApprovedEvent(requestSub));

        _unitOfWork.Repository<RequestSub>().Update(requestSub);
        await _unitOfWork.CompleteAsync();

        await _publishEndpoint.Publish(new RequestApprovedEvent(requestSub), cancellationToken);

        return Unit.Value;
    }
}
