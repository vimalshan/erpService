using MediatR;
using MasterService.Application.DTOs;

namespace MasterService.Application.Features.Trainings.Queries;

public record GetTrainingsQuery : IRequest<IEnumerable<TrainingProviderDto>>;
public record GetTrainingByCodeQuery(long TrainingCode) : IRequest<TrainingProviderDto?>;
