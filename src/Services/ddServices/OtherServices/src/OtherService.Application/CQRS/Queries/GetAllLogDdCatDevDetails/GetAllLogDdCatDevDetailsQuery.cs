using MediatR;
using OtherService.Application.DTOs;

namespace OtherService.Application.CQRS.Queries.GetAllLogDdCatDevDetails;

public sealed record GetAllLogDdCatDevDetailsQuery() : IRequest<IEnumerable<LogDdCatDevDetailDto>>;
