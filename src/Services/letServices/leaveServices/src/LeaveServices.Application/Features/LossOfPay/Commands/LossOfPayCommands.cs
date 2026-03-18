using MediatR;
using LeaveServices.Application.DTOs;

namespace LeaveServices.Application.Features.LossOfPay.Commands;

public record RecordLossOfPayCommand(
    long EmpSysId,
    int LopDays,
    DateOnly LopMonth,
    string? Remarks,
    long RecordedBy) : IRequest<LossOfPayDto>;
