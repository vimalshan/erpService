using MediatR;
using OtherService.Application.DTOs;

namespace OtherService.Application.CQRS.Commands.CreateLogDdCatDevDetail;

public sealed record CreateLogDdCatDevDetailCommand(
    decimal? ReqNum,
    decimal? QtnNum,
    decimal? AnsSrl,
    string AppId,
    decimal AppNum,
    DateTime? EntDat,
    string? Desc,
    string? Need) : IRequest<LogDdCatDevDetailDto>;
