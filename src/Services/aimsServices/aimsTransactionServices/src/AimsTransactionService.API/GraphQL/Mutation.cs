using MediatR;
using AimsTransactionService.Application.DTOs;
using AimsTransactionService.Application.Swipes.Commands.RecordSwipe;
using AimsTransactionService.Application.Leaves.Commands.ApplyLeave;
using AimsTransactionService.Application.Leaves.Commands.ApproveLeave;
using AimsTransactionService.Application.Attendance.Commands.ProcessAttendanceBatch;
using AimsTransactionService.Application.CompOffs.Commands.RequestCompOff;

namespace AimsTransactionService.API.GraphQL;

public class Mutation
{
    [GraphQLDescription("Record a biometric swipe punch.")]
    public async Task<SwipeDto> RecordSwipe(
        [Service] ISender sender,
        RecordSwipeInput input,
        CancellationToken cancellationToken)
    {
        var command = new RecordSwipeCommand(
            input.EmployeeSysId, input.GateNo, input.PunchTime,
            input.PunchStatus.Length > 0 ? input.PunchStatus[0] : 'I',
            input.MachineNo, input.ReferenceNo, input.UpdatedBy);

        return await sender.Send(command, cancellationToken);
    }

    [GraphQLDescription("Apply for leave.")]
    public async Task<LeaveDetailDto> ApplyLeave(
        [Service] ISender sender,
        ApplyLeaveInput input,
        CancellationToken cancellationToken)
    {
        var command = new ApplyLeaveCommand(
            input.EmployeeSysId, input.LeaveId, input.FromDate, input.ToDate,
            input.LeaveDays, input.Reason, input.AppliedBy);

        return await sender.Send(command, cancellationToken);
    }

    [GraphQLDescription("Approve or reject a leave application.")]
    public async Task<bool> ApproveLeave(
        [Service] ISender sender,
        long leaveDetailId,
        bool isApproved,
        string? remarks,
        long processedBy,
        CancellationToken cancellationToken)
    {
        await sender.Send(
            new ApproveLeaveCommand(leaveDetailId, isApproved, remarks, processedBy),
            cancellationToken);
        return true;
    }

    [GraphQLDescription("Process attendance batch for a month.")]
    public async Task<AttendanceBatchDto> ProcessAttendanceBatch(
        [Service] ISender sender,
        DateTime monthStart,
        DateTime monthEnd,
        long createdBy,
        CancellationToken cancellationToken)
        => await sender.Send(
            new ProcessAttendanceBatchCommand(monthStart, monthEnd, createdBy),
            cancellationToken);

    [GraphQLDescription("Request comp off.")]
    public async Task<CompOffDto> RequestCompOff(
        [Service] ISender sender,
        long employeeSysId,
        decimal hoursRequested,
        long requestedBy,
        CancellationToken cancellationToken)
        => await sender.Send(
            new RequestCompOffCommand(employeeSysId, hoursRequested, requestedBy),
            cancellationToken);
}

public sealed record RecordSwipeInput(
    long EmployeeSysId,
    int GateNo,
    DateTime PunchTime,
    string PunchStatus,      // "I" or "O"
    int? MachineNo,
    string? ReferenceNo,
    long UpdatedBy);

public sealed record ApplyLeaveInput(
    long EmployeeSysId,
    int LeaveId,
    DateTime FromDate,
    DateTime ToDate,
    decimal LeaveDays,
    string? Reason,
    long AppliedBy);
