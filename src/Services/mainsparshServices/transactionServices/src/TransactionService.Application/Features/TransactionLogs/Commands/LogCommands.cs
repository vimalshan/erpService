using MediatR;
using TransactionService.Application.DTOs;

namespace TransactionService.Application.Features.TransactionLogs.Commands;

public record LogTransactionCommand(
    string TransactionType,
    long TransactionId,
    string Action,
    long ActionBy,
    string? ActionData,
    string? PreviousStatus,
    string? NewStatus,
    string? IpAddress
) : IRequest<TransactionLogDto>;
