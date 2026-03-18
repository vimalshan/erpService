using MediatR;
using LoanManagement.Application.DTOs;

namespace LoanManagement.Application.Commands.AddRepaymentSchedule;

public record AddRepaymentScheduleCommand(
    decimal LoanId,
    List<RepaymentLineItem> Lines
) : IRequest<List<RepaymentScheduleDto>>;

public record RepaymentLineItem(DateTime RepayDate, decimal Amount);
