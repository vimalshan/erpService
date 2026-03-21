using InsuranceService.Application.DTOs;
using MediatR;

namespace InsuranceService.Application.Queries;

public record GetInsuranceDetailsQuery(
    string? CompanyCode,
    long? PlanNumber) : IRequest<IReadOnlyList<TravelInsuranceDto>>;
