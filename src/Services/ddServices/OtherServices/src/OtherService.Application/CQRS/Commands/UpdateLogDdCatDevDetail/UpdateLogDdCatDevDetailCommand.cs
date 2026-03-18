using MediatR;
using OtherService.Application.DTOs;

namespace OtherService.Application.CQRS.Commands.UpdateLogDdCatDevDetail;

public sealed record UpdateLogDdCatDevDetailCommand(
    string AppId,
    decimal AppNum,
    decimal? ReqNum,
    decimal? QtnNum,
    decimal? AnsSrl,
    DateTime? EntDat,
    string? Desc,
    string? Need) : IRequest<LogDdCatDevDetailDto?>;
