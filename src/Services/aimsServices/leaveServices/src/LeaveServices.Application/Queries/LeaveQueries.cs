using MediatR;
using LeaveServices.Application.DTOs;

namespace LeaveServices.Application.Queries.Leave;

public record GetLeaveDetailByIdQuery(long LeaveDetailId)             : IRequest<LeaveDetailsDto?>;
public record GetLeavesByEmployeeQuery(long EmpSysId)                  : IRequest<IEnumerable<LeaveDetailsDto>>;
public record GetPendingLeavesQuery()                                   : IRequest<IEnumerable<LeaveDetailsDto>>;
public record GetLeaveMasterQuery()                                     : IRequest<IEnumerable<LeaveMasterDto>>;
public record GetLeaveMasterByIdQuery(long LeaveId)                    : IRequest<LeaveMasterDto?>;
public record GetLeaveBalanceQuery(long EmpSysId, long LeaveId)        : IRequest<decimal>;
public record GetLeaveBalanceAllQuery(long EmpSysId, int Year)         : IRequest<IEnumerable<LeaveCreditDto>>;
public record GetLeaveApprovalHistoryQuery(long LeaveDetailId)         : IRequest<IEnumerable<LeaveApprovalDto>>;
public record GetCompOffByEmployeeQuery(long EmpSysId)                 : IRequest<IEnumerable<CompOffDto>>;
