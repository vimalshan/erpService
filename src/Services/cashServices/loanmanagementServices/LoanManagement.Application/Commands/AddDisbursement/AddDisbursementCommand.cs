using MediatR;
using LoanManagement.Application.DTOs;

namespace LoanManagement.Application.Commands.AddDisbursement;

public record AddDisbursementCommand(
    decimal LoanId,
    DateTime DisbDate,
    decimal Amount,
    decimal? ExcRate = null
) : IRequest<DisbursementScheduleDto>;
