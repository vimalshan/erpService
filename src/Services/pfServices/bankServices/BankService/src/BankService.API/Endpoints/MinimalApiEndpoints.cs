using BankService.Application.Commands.BankMasters;
using BankService.Application.Commands.Cheques;
using BankService.Application.Queries.BankAccounts;
using BankService.Application.Queries.BankMasters;
using BankService.Application.Queries.Cheques;
using MediatR;

namespace BankService.API.Endpoints;

public static class MinimalApiEndpoints
{
    public static void MapBankMinimalApis(this WebApplication app)
    {
        var bankGroup = app.MapGroup("/api/minimal/banks")
            .WithTags("Banks (Minimal)")
            .RequireAuthorization();

        bankGroup.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllBankMastersQuery(), ct)));

        bankGroup.MapGet("/{trustCode}/{bankCode}", async (string trustCode, string bankCode,
            IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetBankMasterByCodeQuery(trustCode, bankCode), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        var accountGroup = app.MapGroup("/api/minimal/accounts")
            .WithTags("Accounts (Minimal)")
            .RequireAuthorization();

        accountGroup.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllBankAccountsQuery(), ct)));

        accountGroup.MapGet("/{accountId:long}", async (long accountId,
            IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetBankAccountByIdQuery(accountId), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        var chequeGroup = app.MapGroup("/api/minimal/cheques")
            .WithTags("Cheques (Minimal)")
            .RequireAuthorization();

        chequeGroup.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllChequesQuery(), ct)));

        chequeGroup.MapGet("/{chequeId:long}", async (long chequeId,
            IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetChequeByIdQuery(chequeId), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });
    }
}
