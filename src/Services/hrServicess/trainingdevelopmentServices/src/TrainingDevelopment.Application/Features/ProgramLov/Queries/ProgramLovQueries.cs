using MediatR;
using TrainingDevelopment.Application.DTOs;

namespace TrainingDevelopment.Application.Features.ProgramLov.Queries;

public record GetProgramLovListQuery : IRequest<IEnumerable<ProgramLovDto>>;
public record GetProgramLovByTypeCodeQuery(string TypeCode) : IRequest<ProgramLovDto>;
