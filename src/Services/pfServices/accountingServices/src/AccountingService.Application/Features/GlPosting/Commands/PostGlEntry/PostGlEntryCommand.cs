using AccountingService.Application.DTOs;
using MediatR;

namespace AccountingService.Application.Features.GlPosting.Commands.PostGlEntry;

public record PostGlEntryCommand(
    string AccountCode,
    DateTime PostingDate,
    decimal DebitAmount,
    decimal CreditAmount,
    long ReferenceId,
    string? Remarks
) : IRequest<GlPostingDto>;
