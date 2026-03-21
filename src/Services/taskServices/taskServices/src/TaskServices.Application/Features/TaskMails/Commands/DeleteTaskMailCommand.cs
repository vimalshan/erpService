using MediatR;

namespace TaskServices.Application.Features.TaskMails.Commands;

public record DeleteTaskMailCommand(decimal MID) : IRequest<Unit>;
