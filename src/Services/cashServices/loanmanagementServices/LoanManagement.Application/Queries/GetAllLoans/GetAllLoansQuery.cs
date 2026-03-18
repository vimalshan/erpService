using MediatR;
using LoanManagement.Application.DTOs;

namespace LoanManagement.Application.Queries.GetAllLoans;

public record GetAllLoansQuery(decimal? OrgId = null) : IRequest<IEnumerable<LoanDto>>;
