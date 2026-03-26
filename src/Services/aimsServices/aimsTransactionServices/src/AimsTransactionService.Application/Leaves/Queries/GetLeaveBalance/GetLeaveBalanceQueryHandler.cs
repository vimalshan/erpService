using MediatR;
using AimsTransactionService.Application.Common.Interfaces;
using AimsTransactionService.Application.DTOs;

namespace AimsTransactionService.Application.Leaves.Queries.GetLeaveBalance;

public sealed class GetLeaveBalanceQueryHandler(ILeaveCreditRepository leaveCreditRepository)
    : IRequestHandler<GetLeaveBalanceQuery, LeaveBalanceDto>
{
    public async Task<LeaveBalanceDto> Handle(
        GetLeaveBalanceQuery request, CancellationToken cancellationToken)
    {
        var balance = await leaveCreditRepository.GetBalanceAsync(
            request.EmployeeSysId, request.LeaveId, cancellationToken);

        return new LeaveBalanceDto(request.EmployeeSysId, request.LeaveId, balance);
    }
}
