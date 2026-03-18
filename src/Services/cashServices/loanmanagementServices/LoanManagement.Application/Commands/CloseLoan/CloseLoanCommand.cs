using MediatR;

namespace LoanManagement.Application.Commands.CloseLoan;

public record CloseLoanCommand(decimal LoanId, decimal ModifiedBy) : IRequest<bool>;
