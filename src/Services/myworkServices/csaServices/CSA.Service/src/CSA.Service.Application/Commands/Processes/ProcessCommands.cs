using CSA.Service.Application.DTOs;
using MediatR;

namespace CSA.Service.Application.Commands.Processes;

public record CreateProcessCommand(CreateProcessDto Dto, long UserId) : IRequest<ProcessDto>;
public record CreateSubProcessCommand(CreateSubProcessDto Dto, long UserId) : IRequest<SubProcessDto>;
public record DeleteProcessCommand(long ProcessId) : IRequest<bool>;
