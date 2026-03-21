namespace TransactionService.Application.Commands.ApproveRequest;

using MediatR;
using TransactionService.Domain.Entities;
using TransactionService.Domain.Exceptions;
using TransactionService.Domain.Interfaces;

public sealed class ApproveRequestCommandHandler : IRequestHandler<ApproveRequestCommand, bool>
{
    private readonly IRequestRepository _requestRepository;
    private readonly IBudgetRepository _budgetRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ApproveRequestCommandHandler(
        IRequestRepository requestRepository,
        IBudgetRepository budgetRepository,
        IUnitOfWork unitOfWork)
    {
        _requestRepository = requestRepository;
        _budgetRepository = budgetRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(ApproveRequestCommand request, CancellationToken cancellationToken)
    {
        // Find the request sub first to get the parent request
        var allRequests = await _requestRepository.GetAllAsync(cancellationToken);
        RequestMain? requestMain = null;

        foreach (var rm in allRequests)
        {
            var full = await _requestRepository.GetByIdWithDetailsAsync(rm.RequestId, cancellationToken);
            if (full?.Details.Any(d => d.RequestSubId == request.RequestSubId) == true)
            {
                requestMain = full;
                break;
            }
        }

        if (requestMain is null)
            throw new TransactionDomainException($"Request sub {request.RequestSubId} not found.");

        requestMain.ApproveDetail(
            request.RequestSubId,
            request.ApprovedQty,
            request.ApproverSysId,
            request.Remarks);

        await _unitOfWork.CompleteAsync(cancellationToken);
        return true;
    }
}
