using MediatR;
using AimsTransactionService.Application.DTOs;
using AimsTransactionService.Application.Swipes.Queries.GetSwipesByEmployee;
using AimsTransactionService.Application.Attendance.Queries.GetAttendanceSummary;
using AimsTransactionService.Application.Leaves.Queries.GetLeavesByEmployee;
using AimsTransactionService.Application.Leaves.Queries.GetLeaveBalance;
using AimsTransactionService.Application.CompOffs.Queries.GetCompOffsByEmployee;

namespace AimsTransactionService.API.GraphQL;

public class Query
{
    [GraphQLDescription("Get swipes for an employee within a date range.")]
    public async Task<IEnumerable<SwipeDto>> GetSwipesByEmployee(
        [Service] ISender sender,
        long employeeSysId,
        DateTime fromDate,
        DateTime toDate,
        CancellationToken cancellationToken)
        => await sender.Send(new GetSwipesByEmployeeQuery(employeeSysId, fromDate, toDate), cancellationToken);

    [GraphQLDescription("Get attendance summary for an employee.")]
    public async Task<AttendanceSummaryDto?> GetAttendanceSummary(
        [Service] ISender sender,
        long employeeSysId,
        DateTime monthStart,
        DateTime monthEnd,
        CancellationToken cancellationToken)
        => await sender.Send(new GetAttendanceSummaryQuery(employeeSysId, monthStart, monthEnd), cancellationToken);

    [GraphQLDescription("Get leave applications for an employee.")]
    public async Task<IEnumerable<LeaveDetailDto>> GetLeavesByEmployee(
        [Service] ISender sender,
        long employeeSysId,
        CancellationToken cancellationToken)
        => await sender.Send(new GetLeavesByEmployeeQuery(employeeSysId), cancellationToken);

    [GraphQLDescription("Get leave balance for an employee and leave type.")]
    public async Task<LeaveBalanceDto> GetLeaveBalance(
        [Service] ISender sender,
        long employeeSysId,
        int leaveId,
        CancellationToken cancellationToken)
        => await sender.Send(new GetLeaveBalanceQuery(employeeSysId, leaveId), cancellationToken);

    [GraphQLDescription("Get comp off requests for an employee.")]
    public async Task<IEnumerable<CompOffDto>> GetCompOffsByEmployee(
        [Service] ISender sender,
        long employeeSysId,
        CancellationToken cancellationToken)
        => await sender.Send(new GetCompOffsByEmployeeQuery(employeeSysId), cancellationToken);
}
