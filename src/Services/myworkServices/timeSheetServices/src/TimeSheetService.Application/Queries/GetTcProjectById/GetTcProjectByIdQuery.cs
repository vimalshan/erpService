using MediatR;
using TimeSheetService.Application.DTOs;

namespace TimeSheetService.Application.Queries.GetTcProjectById;

public record GetTcProjectByIdQuery(long ProjectId) : IRequest<TcProjectDto?>;
