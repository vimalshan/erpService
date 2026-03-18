using MediatR;
using MasterService.Application.DTOs;

namespace MasterService.Application.Features.Trainings.Commands;

public record CreateTrainingCommand(
    long TrainingCode,
    string TrainingName,
    string? Address1,
    string? ContactName,
    string? PhoneNum,
    long? GroupCode) : IRequest<TrainingProviderDto>;

public record CancelTrainingCommand(long TrainingCode, string? CancelRemark) : IRequest<Unit>;
public record UpdateTrainingBrochureCommand(long TrainingCode, string FilePath) : IRequest<Unit>;
