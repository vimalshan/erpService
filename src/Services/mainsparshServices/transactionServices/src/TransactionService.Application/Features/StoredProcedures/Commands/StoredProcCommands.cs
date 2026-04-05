using MediatR;
using TransactionService.Application.DTOs;

namespace TransactionService.Application.Features.StoredProcedures.Commands;

public record ProcessMonthlyStipendSpCommand(
    int Month,
    int Year,
    long ProcessedBy
) : IRequest<StoredProcResultDto>;
