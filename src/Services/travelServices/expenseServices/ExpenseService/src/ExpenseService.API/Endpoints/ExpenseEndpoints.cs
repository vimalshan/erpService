using ExpenseService.Application.Commands;
using ExpenseService.Application.Queries;
using MediatR;

namespace ExpenseService.API.Endpoints;

public static class ExpenseEndpoints
{
    public static void MapExpenseEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v2/expenses")
            .WithTags("Expenses (Minimal API)")
            .RequireAuthorization();

        group.MapGet("/{requestNumber}/{serialNumber}", async (
            long requestNumber, long serialNumber, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetExpenseByIdQuery
            {
                RequestNumber = requestNumber,
                SerialNumber = serialNumber
            });
            return result is not null ? Results.Ok(result) : Results.NotFound();
        })
        .WithName("GetExpenseById_v2")
        .Produces(200).Produces(404);

        group.MapGet("/request/{requestNumber}", async (
            long requestNumber, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetExpensesByRequestQuery { RequestNumber = requestNumber });
            return Results.Ok(result);
        })
        .WithName("GetExpensesByRequest_v2")
        .Produces(200);

        group.MapPost("/", async (RecordExpenseCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return Results.Created($"/api/v2/expenses/{result.RequestNumber}/{result.SerialNumber}", result);
        })
        .WithName("CreateExpense_v2")
        .Produces(201).Produces(400);

        group.MapDelete("/{requestNumber}/{serialNumber}", async (
            long requestNumber, long serialNumber, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteExpenseCommand
            {
                RequestNumber = requestNumber,
                SerialNumber = serialNumber
            });
            return result ? Results.NoContent() : Results.NotFound();
        })
        .WithName("DeleteExpense_v2")
        .Produces(204).Produces(404);

        group.MapGet("/summary/{requestNumber}", async (
            long requestNumber, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetExpenseSummaryQuery { RequestNumber = requestNumber });
            return result is not null ? Results.Ok(result) : Results.NotFound();
        })
        .WithName("GetExpenseSummary_v2")
        .Produces(200).Produces(404);

        group.MapPost("/da/calculate", async (CalculateDACommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return Results.Ok(result);
        })
        .WithName("CalculateDA_v2")
        .Produces(200).Produces(400);

        group.MapPost("/settle/{requestNumber}", async (
            long requestNumber, IMediator mediator) =>
        {
            var result = await mediator.Send(new SettleExpensesCommand { RequestNumber = requestNumber });
            return Results.Ok(result);
        })
        .WithName("SettleExpenses_v2")
        .Produces(200).Produces(400);
    }
}
