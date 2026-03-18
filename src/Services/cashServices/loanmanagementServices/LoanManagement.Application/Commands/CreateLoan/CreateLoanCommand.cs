using MediatR;
using LoanManagement.Application.DTOs;

namespace LoanManagement.Application.Commands.CreateLoan;

public record CreateLoanCommand(
    string LoanKey,
    decimal OrgId,
    decimal LoanAmount,
    decimal LoanTypeId,
    decimal BankId,
    decimal CreatedBy,
    DateTime LoanDate,
    decimal? OrgCurr = null,
    decimal? LoanCurr = null
) : IRequest<LoanDto>;
