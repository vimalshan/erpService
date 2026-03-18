using LoanService.Application.Common;
using LoanService.Application.DTOs;
using MediatR;

namespace LoanService.Application.Loans.Queries;

public record GetLoanByIdQuery(long LoanNo) : IRequest<Result<LoanDto>>;
public record GetLoansByMemberQuery(long MemberId) : IRequest<Result<IReadOnlyList<LoanDto>>>;
public record GetActiveLoansQuery : IRequest<Result<IReadOnlyList<LoanDto>>>;
public record GetActiveLoansSummaryQuery : IRequest<Result<IEnumerable<ActiveLoanDto>>>;
