using AccountingService.Application.DTOs;
using AccountingService.Application.Features.GlPosting.Commands.PostGlEntry;
using AccountingService.Application.Features.MainAccounts.Commands.CreateMainAccount;
using MediatR;

namespace AccountingService.API.GraphQL;

public class Mutation
{
    /// <summary>Create a new Main Account</summary>
    public async Task<MainAccountDto> CreateMainAccount(
        [Service] IMediator mediator,
        string mainAccountCode, string mainAccountName, string? mainAccountShrtName,
        CancellationToken ct)
        => await mediator.Send(new CreateMainAccountCommand(mainAccountCode, mainAccountName, mainAccountShrtName), ct);

    /// <summary>Post a GL Entry</summary>
    public async Task<GlPostingDto> PostGlEntry(
        [Service] IMediator mediator,
        string accountCode, DateTime postingDate,
        decimal debitAmount, decimal creditAmount,
        long referenceId, string? remarks,
        CancellationToken ct)
        => await mediator.Send(new PostGlEntryCommand(accountCode, postingDate, debitAmount, creditAmount, referenceId, remarks), ct);
}
