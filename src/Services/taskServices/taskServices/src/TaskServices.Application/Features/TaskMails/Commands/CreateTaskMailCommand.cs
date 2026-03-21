using MediatR;

namespace TaskServices.Application.Features.TaskMails.Commands;

public record CreateTaskMailCommand(decimal MID, decimal SYSID) : IRequest<decimal>;
