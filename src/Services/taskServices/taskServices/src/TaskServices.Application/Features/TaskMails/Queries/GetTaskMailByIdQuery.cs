using MediatR;
using TaskServices.Application.DTOs;

namespace TaskServices.Application.Features.TaskMails.Queries;

public record GetTaskMailByIdQuery(decimal MID) : IRequest<TaskMailDto?>;
