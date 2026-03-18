using MediatR;
using TrainingDevelopment.Application.DTOs;

namespace TrainingDevelopment.Application.Features.TrainingDetails.Commands.CreateTrainingDetail;

public record CreateTrainingDetailCommand(
    decimal Id,
    decimal FinancialYear,
    decimal EmployeeSysId,
    string TrainingNeed,
    string GapArea,
    decimal Mode,
    decimal ProgramId,
    string ProgramDescription,
    DateTime PlannedFrom,
    DateTime PlannedTo,
    decimal? LastModifiedBy
) : IRequest<TrainingDetailDto>;
