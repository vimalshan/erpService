using MediatR;
using OtherService.Application.DTOs;

namespace OtherService.Application.CQRS.Queries.GetLogDdCatDevDetailsByReqNum;

public sealed record GetLogDdCatDevDetailsByReqNumQuery(decimal ReqNum)
    : IRequest<IEnumerable<LogDdCatDevDetailDto>>;
