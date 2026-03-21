using MediatR;

namespace TaskServices.Application.Features.TaskMails.Commands;

public record UpdateTaskMailCommand(decimal MID, decimal SYSID) : IRequest<Unit>;
