using MediatR;
using OtherService.Application.DTOs;

namespace OtherService.Application.CQRS.Queries.GetLogDdCatDevDetailByKey;

public sealed record GetLogDdCatDevDetailByKeyQuery(
    string AppId,
    decimal AppNum) : IRequest<LogDdCatDevDetailDto?>;
