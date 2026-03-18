using ContributionService.Application.DTOs;
using MediatR;

namespace ContributionService.Application.Commands.ContributionDetail;

public record CreateContributionDetailCommand(
    decimal BatchNo,
    decimal MemberNo,
    string UnitCode,
    decimal EmployeeNo,
    decimal BasicAmount,
    decimal FpsBasicAmount,
    decimal EeAmount,
    decimal ErAmount,
    decimal VeAmount,
    decimal FpAmount,
    decimal LoanPrincipal,
    decimal LoanInterest,
    string EntByUserId,
    decimal EntEmpSysId,
    string TypeCode
) : IRequest<ContributionDetailDto>;

public record ValidateContributionDetailCommand(decimal ContributionId) : IRequest<string>;
