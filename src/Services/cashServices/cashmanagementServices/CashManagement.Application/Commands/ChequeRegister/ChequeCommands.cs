using MediatR;
using CashManagement.Application.DTOs;

namespace CashManagement.Application.Commands.ChequeRegister;

public record IssueChequeCommand(
    long BankAccountId,
    string ChequeNumber,
    string PayeeName,
    decimal Amount,
    DateOnly ChequeDate,
    string? Reference,
    long IssuedBy
) : IRequest<ChequeDto>;

public record MarkChequeBouncedCommand(
    long ChequeId,
    string BounceReason,
    long ProcessedBy
) : IRequest<bool>;

public record MarkChequeClearedCommand(
    long ChequeId,
    long ProcessedBy
) : IRequest<bool>;

public record CancelChequeCommand(
    long ChequeId,
    long ProcessedBy
) : IRequest<bool>;
