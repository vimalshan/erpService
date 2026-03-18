using MediatR;

namespace TrainingDevelopment.Application.Features.TrainingDetails.Commands.DeleteTrainingDetail;

public record DeleteTrainingDetailCommand(decimal Id) : IRequest<bool>;
