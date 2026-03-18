using LoanDefinition.Application.DTOs;
using MediatR;

namespace LoanDefinition.Application.Features.Loans.Queries;

public record GetAllLoansQuery : IRequest<IReadOnlyList<LoanMasterDto>>;
public record GetLoanByIdQuery(long LoanId) : IRequest<LoanMasterDto?>;
public record GetLoanDetailQuery(long LoanId) : IRequest<LoanMasterDetailDto?>;
public record GetLoansByTypeQuery(long LoanTypeId) : IRequest<IReadOnlyList<LoanMasterDto>>;
public record GetActiveLoansQuery : IRequest<IReadOnlyList<LoanMasterDto>>;
