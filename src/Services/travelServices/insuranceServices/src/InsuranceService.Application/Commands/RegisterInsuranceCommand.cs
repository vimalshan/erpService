using MediatR;

namespace InsuranceService.Application.Commands;

public record RegisterInsuranceCommand(
    string CompanyCode,
    long PlanNumber,
    string InsuranceType,
    string? PassportNumber,
    string? VisaPlace,
    string? Nominee1,
    string? Nominee2,
    string? Remarks) : IRequest<RegisterInsuranceResult>;

public record RegisterInsuranceResult(
    bool Success,
    string Message,
    long? PlanNumber);
