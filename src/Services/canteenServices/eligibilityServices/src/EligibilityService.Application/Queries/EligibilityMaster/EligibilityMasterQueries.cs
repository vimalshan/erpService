using MediatR;
using EligibilityService.Application.DTOs;

namespace EligibilityService.Application.Queries.EligibilityMaster;

public record GetEligibilityMasterQuery(long CanteenUnit, string ShiftCode, decimal ItemCode)
    : IRequest<EligibilityMasterDto?>;

public record GetAllEligibilityMastersQuery(long? CanteenUnit = null)
    : IRequest<IEnumerable<EligibilityMasterDto>>;

public record CheckEmployeeEligibilityQuery(
    long CanteenUnit,
    string ShiftCode,
    decimal ItemCode,
    int RequestedQty) : IRequest<EligibilityCheckResultDto>;

public record GetEligibilityHistoryQuery(long CanteenUnit, string ShiftCode, decimal ItemCode)
    : IRequest<IEnumerable<EligibilityMasterHistoryDto>>;
