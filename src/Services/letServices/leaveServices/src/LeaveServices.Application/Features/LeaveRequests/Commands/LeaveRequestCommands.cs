using MediatR;
using LeaveServices.Application.DTOs;

namespace LeaveServices.Application.Features.LeaveRequests.Commands;

public record CreateLeaveRequestCommand(
    long ReqNum,
    int FinyearSrlno,
    string EmpUserId,
    string? SupUserId) : IRequest<LeaveRequestDto>;

public record AddLeaveRequestDetailCommand(
    long ReqNum,
    int SrlNum,
    string? ModUser,
    char? PrefModDev,
    string? ActTaken) : IRequest<LeaveRequestDetailDto>;
