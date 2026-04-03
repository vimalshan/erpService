using LoanTransaction.Application.Commands;
using LoanTransaction.Application.DTOs;
using LoanTransaction.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace LoanTransaction.API.MinimalApis;

public static class LoanTransactionEndpoints
{
    public static void MapLoanTransactionEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/v1/minimal/loans")
            .WithTags("LoanTransaction (Minimal APIs)")
            .RequireAuthorization();

        group.MapGet("/{loanNo:long}", async (long loanNo, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetLoanByIdQuery(loanNo), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("MinimalGetLoan")
        .Produces<LoanDto>()
        .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/employee/{empId:int}", async (int empId, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetLoansByEmployeeQuery(empId), ct);
            return Results.Ok(result);
        })
        .WithName("MinimalGetLoansByEmployee")
        .Produces<IEnumerable<LoanDto>>();

        group.MapPost("/disburse", async (DisburseLoanCommand cmd, IMediator mediator, CancellationToken ct) =>
        {
            var loanNo = await mediator.Send(cmd, ct);
            return Results.Created($"/api/v1/minimal/loans/{loanNo}", new { loanNo });
        })
        .RequireAuthorization("AdminOrManager")
        .WithName("MinimalDisburseLoan")
        .Produces(StatusCodes.Status201Created);

        group.MapPost("/payment", async (RecordEmiPaymentCommand cmd, IMediator mediator, CancellationToken ct) =>
        {
            await mediator.Send(cmd, ct);
            return Results.NoContent();
        })
        .WithName("MinimalRecordEmiPayment")
        .Produces(StatusCodes.Status204NoContent);

        group.MapPost("/{loanNo}/close", async (string loanNo, CloseLoanCommand cmd, IMediator mediator, CancellationToken ct) =>
        {
            await mediator.Send(cmd, ct);
            return Results.NoContent();
        })
        .RequireAuthorization("AdminOrManager")
        .WithName("MinimalCloseLoan")
        .Produces(StatusCodes.Status204NoContent);

        group.MapPost("/emi/calculate", async (CalculateEmiQuery query, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(query, ct);
            return Results.Ok(result);
        })
        .WithName("MinimalCalculateEmi")
        .Produces<EmiCalculationResultDto>();
    }
}
