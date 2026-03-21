namespace TransactionService.Application.Commands.SubmitRequest;

using MediatR;
using TransactionService.Domain.Entities;
using TransactionService.Domain.Interfaces;

public sealed class SubmitRequestCommandHandler : IRequestHandler<SubmitRequestCommand, long>
{
    private readonly IRequestRepository _requestRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SubmitRequestCommandHandler(IRequestRepository requestRepository, IUnitOfWork unitOfWork)
    {
        _requestRepository = requestRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<long> Handle(SubmitRequestCommand request, CancellationToken cancellationToken)
    {
        var requestId = await _requestRepository.GetNextRequestIdAsync(cancellationToken);

        var requestMain = RequestMain.Create(
            requestId, request.RequestedBy, request.LocationId, request.UnitCode);

        foreach (var item in request.Items)
        {
            var subId = await _requestRepository.GetNextRequestSubIdAsync(cancellationToken);
            var sub = RequestSub.Create(
                subId, requestId, item.StationaryId, item.DeptId,
                item.ExpectedDate, null, item.RequestedQty, request.RequestedBy);
            requestMain.AddDetail(sub);
        }

        await _requestRepository.AddAsync(requestMain, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return requestId;
    }
}
