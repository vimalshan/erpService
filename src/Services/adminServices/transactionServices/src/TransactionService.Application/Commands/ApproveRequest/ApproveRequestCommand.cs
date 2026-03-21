namespace TransactionService.Application.Commands.ApproveRequest;

using MediatR;

public sealed record ApproveRequestCommand(
    long RequestSubId,
    long ApprovedQty,
    long ApproverSysId,
    string? Remarks) : IRequest<bool>;
