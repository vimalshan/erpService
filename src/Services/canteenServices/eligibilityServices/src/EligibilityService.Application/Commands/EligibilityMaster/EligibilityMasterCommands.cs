using MediatR;
using EligibilityService.Application.DTOs;

namespace EligibilityService.Application.Commands.EligibilityMaster;

public record CreateEligibilityMasterCommand(
    long CanteenUnit,
    string ShiftCode,
    decimal ItemCode,
    int? EligibleLimit,
    long? EnteredUser,
    string? TimeOfficeUnit) : IRequest<EligibilityMasterDto>;

public record UpdateEligibilityMasterCommand(
    long CanteenUnit,
    string ShiftCode,
    decimal ItemCode,
    int? EligibleLimit,
    string? TimeOfficeUnit,
    long ModifiedUser) : IRequest<EligibilityMasterDto>;

public record DeleteEligibilityMasterCommand(
    long CanteenUnit,
    string ShiftCode,
    decimal ItemCode) : IRequest<bool>;
