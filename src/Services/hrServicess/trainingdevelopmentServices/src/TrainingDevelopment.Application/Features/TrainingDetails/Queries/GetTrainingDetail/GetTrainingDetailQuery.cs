using MediatR;
using TrainingDevelopment.Application.DTOs;

namespace TrainingDevelopment.Application.Features.TrainingDetails.Queries.GetTrainingDetail;

public record GetTrainingDetailQuery(decimal Id) : IRequest<TrainingDetailDto>;
