using CSA.Service.Application.DTOs;
using MediatR;

namespace CSA.Service.Application.Queries.Processes;

public record GetAllProcessesQuery : IRequest<IEnumerable<ProcessDto>>;
public record GetProcessByIdQuery(long ProcessId) : IRequest<ProcessDto?>;
public record GetSubProcessesByProcessQuery(long ProcessId) : IRequest<IEnumerable<SubProcessDto>>;
