using MediatR;
using TaskServices.Application.DTOs;

namespace TaskServices.Application.Features.TaskMails.Queries;

public record GetAllTaskMailsQuery : IRequest<IReadOnlyList<TaskMailDto>>;
