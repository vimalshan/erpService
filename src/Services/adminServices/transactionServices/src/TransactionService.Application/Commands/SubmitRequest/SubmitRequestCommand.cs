namespace TransactionService.Application.Commands.SubmitRequest;

using MediatR;

public sealed record SubmitRequestCommand(
    long RequestedBy,
    long LocationId,
    string UnitCode,
    List<RequestItemDto> Items) : IRequest<long>;

public sealed record RequestItemDto(
    long StationaryId,
    long DeptId,
    DateTime ExpectedDate,
    long RequestedQty,
    string? Remarks);
