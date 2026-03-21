using MediatR;
using TaskServices.Application.DTOs;

namespace TaskServices.Application.Features.TaskMails.Queries;

public record GetTaskMailsBySystemUserQuery(decimal SYSID) : IRequest<IReadOnlyList<TaskMailDto>>;
