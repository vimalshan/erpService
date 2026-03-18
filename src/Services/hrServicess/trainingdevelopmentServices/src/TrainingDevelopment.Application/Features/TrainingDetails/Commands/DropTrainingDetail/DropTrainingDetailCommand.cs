using MediatR;

namespace TrainingDevelopment.Application.Features.TrainingDetails.Commands.DropTrainingDetail;

public record DropTrainingDetailCommand(decimal Id, string Remarks) : IRequest<bool>;
