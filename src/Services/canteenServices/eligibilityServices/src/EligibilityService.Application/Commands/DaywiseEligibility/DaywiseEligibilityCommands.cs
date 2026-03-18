using MediatR;
using EligibilityService.Application.DTOs;

namespace EligibilityService.Application.Commands.DaywiseEligibility;

public record CreateDaywiseEligibilityCommand(
    long SerialNumber,
    long CompanyCode,
    long EmployeeSysId,
    DateTime? AttendanceDate,
    long? ProcessNumber,
    string? ShiftCode,
    long? ItemCode,
    int? ShiftQuantity,
    int? BeforeShiftQty,
    int? AfterShiftQty,
    long? EnteredUser,
    string? FlexField1,
    string? GradeType) : IRequest<DaywiseEligibilityDto>;

public record DeleteDaywiseEligibilityCommand(long SerialNumber) : IRequest<bool>;
