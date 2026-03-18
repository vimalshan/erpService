using MediatR;
using LoanManagement.Application.DTOs;

namespace LoanManagement.Application.Queries.GetLoanById;

public record GetLoanByIdQuery(decimal LoanId) : IRequest<LoanDto?>;
