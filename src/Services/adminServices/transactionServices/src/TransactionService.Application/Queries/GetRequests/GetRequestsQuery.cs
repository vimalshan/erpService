namespace TransactionService.Application.Queries.GetRequests;

using MediatR;
using TransactionService.Application.DTOs;

public sealed record GetAllRequestsQuery(long? LocationId = null) : IRequest<IEnumerable<RequestSummaryDto>>;

public sealed record GetRequestByIdQuery(long RequestId) : IRequest<RequestMainDto?>;

public sealed record GetRequestsByEmployeeQuery(long EmpSysId) : IRequest<IEnumerable<RequestSummaryDto>>;
