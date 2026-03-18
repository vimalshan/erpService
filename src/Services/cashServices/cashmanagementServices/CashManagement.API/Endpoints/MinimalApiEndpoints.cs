using MediatR;
using CashManagement.Application.Commands.CashUnit;
using CashManagement.Application.Commands.BankAccount;
using CashManagement.Application.Queries.CashUnit;
using CashManagement.Application.Queries.BankAccount;

namespace CashManagement.API.Endpoints;

public static class MinimalApiEndpoints
{
    public static void MapCashManagementEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/v1/minimal").RequireAuthorization();

        // Cash Unit quick endpoints
        group.MapGet("/cash-units", async (IMediator mediator, CancellationToken ct)
            => Results.Ok(await mediator.Send(new GetAllCashUnitsQuery(), ct)))
            .WithName("MinimalGetCashUnits")
            .WithSummary("Get all cash units");

        group.MapGet("/cash-units/{id:long}/balance", async (long id, IMediator mediator, CancellationToken ct)
            => Results.Ok(await mediator.Send(new GetCashInHandQuery(id, DateTime.UtcNow), ct)))
            .WithName("MinimalGetCashBalance")
            .WithSummary("Get cash in hand for a unit");

        // Bank Account quick endpoints
        group.MapGet("/bank-accounts", async (IMediator mediator, CancellationToken ct)
            => Results.Ok(await mediator.Send(new GetAllBankAccountsQuery(), ct)))
            .WithName("MinimalGetBankAccounts")
            .WithSummary("Get all bank accounts");

        group.MapGet("/bank-accounts/{id:long}/balance", async (long id, IMediator mediator, CancellationToken ct)
            => Results.Ok(await mediator.Send(new GetBankBalanceQuery(id, DateTime.UtcNow), ct)))
            .WithName("MinimalGetBankBalance")
            .WithSummary("Get bank balance");
    }
}
