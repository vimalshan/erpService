using MediatR;
using TimeSheetService.Application.DTOs;

namespace TimeSheetService.Application.Queries.GetTsProjects;

public record GetTsProjectsQuery : IRequest<IEnumerable<TsProjectDto>>;
