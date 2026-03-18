using MediatR;
using EligibilityService.Application.DTOs;

namespace EligibilityService.Application.Queries.DaywiseEligibility;

public record GetDaywiseEligibilityQuery(long SerialNumber)
    : IRequest<DaywiseEligibilityDto?>;

public record GetDaywiseEligibilityByEmployeeQuery(long CompanyCode, long EmployeeSysId)
    : IRequest<IEnumerable<DaywiseEligibilityDto>>;

public record GetDaywiseEligibilityByDateQuery(long CompanyCode, DateTime Date)
    : IRequest<IEnumerable<DaywiseEligibilityDto>>;
