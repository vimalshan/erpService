using MediatR;

namespace OtherService.Application.CQRS.Commands.DeleteLogDdCatDevDetail;

public sealed record DeleteLogDdCatDevDetailCommand(
    string AppId,
    decimal AppNum) : IRequest<bool>;
