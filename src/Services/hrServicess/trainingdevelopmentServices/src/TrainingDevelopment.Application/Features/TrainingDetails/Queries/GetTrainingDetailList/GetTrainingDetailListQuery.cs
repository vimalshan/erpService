using MediatR;
using TrainingDevelopment.Application.DTOs;

namespace TrainingDevelopment.Application.Features.TrainingDetails.Queries.GetTrainingDetailList;

public record GetTrainingDetailListQuery(
    decimal? EmployeeSysId = null,
    decimal? FinancialYear = null,
    string? Status = null
) : IRequest<IEnumerable<TrainingDetailDto>>;
