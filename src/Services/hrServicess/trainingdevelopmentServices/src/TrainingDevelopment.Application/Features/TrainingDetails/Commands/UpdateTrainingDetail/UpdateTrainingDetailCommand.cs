using MediatR;
using TrainingDevelopment.Application.DTOs;

namespace TrainingDevelopment.Application.Features.TrainingDetails.Commands.UpdateTrainingDetail;

public record UpdateTrainingDetailCommand(
    decimal Id,
    string TrainingNeed,
    string GapArea,
    decimal Mode,
    decimal ProgramId,
    string ProgramDescription,
    DateTime PlannedFrom,
    DateTime PlannedTo,
    decimal? LastModifiedBy
) : IRequest<TrainingDetailDto>;
