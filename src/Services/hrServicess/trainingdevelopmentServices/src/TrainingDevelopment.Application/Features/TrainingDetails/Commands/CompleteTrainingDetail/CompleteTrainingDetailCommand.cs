using MediatR;

namespace TrainingDevelopment.Application.Features.TrainingDetails.Commands.CompleteTrainingDetail;

public record CompleteTrainingDetailCommand(
    decimal Id,
    DateTime ActualFrom,
    DateTime ActualTo,
    decimal? InstituteId,
    string? InstituteDescription,
    decimal? TrainerId,
    string? TrainerDescription,
    decimal? PlaceId,
    string? Place,
    decimal? Cost
) : IRequest<bool>;
