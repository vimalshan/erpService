using LoanService.Application.DTOs;
using LoanService.Application.Loans.Commands;
using LoanService.Application.Loans.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LoanService.Api.Endpoints;

public static class LoanEndpoints
{
    public static void MapLoanEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/minimal/loans")
            .WithTags("Loans (Minimal API)")
            .RequireAuthorization();

        group.MapGet("/{loanNo:long}", async (long loanNo, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetLoanByIdQuery(loanNo), ct);
            return result.IsSuccess ? Results.Ok(result.Data) : Results.NotFound(result.Error);
        })
        .WithName("GetLoanMinimal")
        .Produces<LoanDto>(200)
        .Produces(404);

        group.MapGet("/active", async (IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetActiveLoansQuery(), ct);
            return Results.Ok(result.Data);
        })
        .WithName("GetActiveLoansMinimal")
        .Produces<IReadOnlyList<LoanDto>>(200);

        group.MapGet("/member/{memberId:long}", async (long memberId, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetLoansByMemberQuery(memberId), ct);
            return Results.Ok(result.Data);
        })
        .WithName("GetLoansByMemberMinimal")
        .Produces<IReadOnlyList<LoanDto>>(200);

        group.MapPost("/", async ([FromBody] CreateLoanCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return result.IsSuccess
                ? Results.Created($"/api/minimal/loans/{result.Data!.LoanNo}", result.Data)
                : Results.BadRequest(result.Error);
        })
        .WithName("CreateLoanMinimal")
        .Produces<LoanDto>(201)
        .Produces(400);

        group.MapPut("/{loanNo:long}/approve", async (long loanNo, [FromBody] DateTime approvalDate, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new ApproveLoanCommand { LoanNo = loanNo, ApprovalDate = approvalDate }, ct);
            return result.IsSuccess ? Results.Ok(result.Data) : Results.NotFound(result.Error);
        })
        .WithName("ApproveLoanMinimal")
        .Produces<LoanDto>(200);

        group.MapPut("/{loanNo:long}/close", async (long loanNo, [FromBody] DateTime closureDate, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new CloseLoanCommand { LoanNo = loanNo, ClosureDate = closureDate }, ct);
            return result.IsSuccess ? Results.Ok(result.Data) : Results.NotFound(result.Error);
        })
        .WithName("CloseLoanMinimal")
        .Produces<LoanDto>(200);
    }
}
