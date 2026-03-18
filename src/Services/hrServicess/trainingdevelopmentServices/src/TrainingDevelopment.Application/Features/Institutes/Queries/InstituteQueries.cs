using MediatR;
using TrainingDevelopment.Application.DTOs;

namespace TrainingDevelopment.Application.Features.Institutes.Queries;

public record GetInstituteListQuery : IRequest<IEnumerable<InstituteMasterDto>>;
public record GetInstituteByCodeQuery(decimal Code) : IRequest<InstituteMasterDto>;
