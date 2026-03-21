using MediatR;

namespace InsuranceService.Application.Commands;

public record UpdateInsuranceStatusCommand(
    string CompanyCode,
    long PlanNumber,
    string Status,
    string? CertificateNumber,
    long? UpdatedBy) : IRequest<UpdateInsuranceStatusResult>;

public record UpdateInsuranceStatusResult(
    bool Success,
    string Message);
