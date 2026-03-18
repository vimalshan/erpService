using CSA.Service.Application.DTOs;
using MediatR;

namespace CSA.Service.Application.Commands.Controls;

public record CreateControlCommand(CreateControlDto Dto, long UserId) : IRequest<ControlDto>;

public record UpdateControlCommand(UpdateControlDto Dto, long UserId) : IRequest<ControlDto>;

public record DeleteControlCommand(long ControlId) : IRequest<bool>;
