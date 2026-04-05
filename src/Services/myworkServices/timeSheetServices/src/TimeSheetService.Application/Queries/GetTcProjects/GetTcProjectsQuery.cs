using MediatR;
using TimeSheetService.Application.DTOs;

namespace TimeSheetService.Application.Queries.GetTcProjects;

public record GetTcProjectsQuery : IRequest<IEnumerable<TcProjectDto>>;
