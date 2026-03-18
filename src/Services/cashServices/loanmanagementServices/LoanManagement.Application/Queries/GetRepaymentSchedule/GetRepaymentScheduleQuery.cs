using MediatR;
using LoanManagement.Application.DTOs;

namespace LoanManagement.Application.Queries.GetRepaymentSchedule;

public record GetRepaymentScheduleQuery(decimal LoanId) : IRequest<IEnumerable<RepaymentScheduleDto>>;
