using TaskTransactional.Application.Commands;
using TaskTransactional.Application.Queries;
using MediatR;

namespace TaskTransactional.API.Endpoints;

public static class ComplaintEndpoints
{
    public static void MapComplaintEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/v2/complaints").RequireAuthorization();

        // Complaint Main
        group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllComplaintMainsQuery(), ct)));

        group.MapGet("/{groupId}", async (string groupId, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetComplaintMainByGroupIdQuery(groupId), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        group.MapGet("/unit/{unitCode}", async (string unitCode, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetComplaintMainsByUnitCodeQuery(unitCode), ct)));

        group.MapPost("/", async (CreateComplaintMainCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var groupId = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/complaints/{groupId}", groupId);
        });

        // Tickets
        var tickets = routes.MapGroup("/api/v2/tickets").RequireAuthorization();

        tickets.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllTicketsQuery(), ct)));

        tickets.MapGet("/{ticketNum}", async (decimal ticketNum, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetTicketByNumQuery(ticketNum), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        tickets.MapGet("/group/{groupId}", async (decimal groupId, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetTicketsByGroupIdQuery(groupId), ct)));

        tickets.MapPost("/", async (CreateTicketCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var ticketNum = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/tickets/{ticketNum}", ticketNum);
        });

        tickets.MapPost("/{ticketNum}/close", async (decimal ticketNum, IMediator mediator, CancellationToken ct) =>
            await mediator.Send(new CloseTicketCommand(ticketNum), ct) ? Results.NoContent() : Results.NotFound());

        // Actions
        var actions = routes.MapGroup("/api/v2/actions").RequireAuthorization();

        actions.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllActionsQuery(), ct)));

        actions.MapGet("/{actionNum}", async (decimal actionNum, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetActionByNumQuery(actionNum), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        actions.MapPost("/", async (CreateActionCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var actionNum = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/actions/{actionNum}", actionNum);
        });

        // Escalations
        var escalations = routes.MapGroup("/api/v2/escalations").RequireAuthorization();

        escalations.MapGet("/ticket/{ticketNum}", async (decimal ticketNum, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetEscalationsByTicketNumQuery(ticketNum), ct)));

        escalations.MapPost("/", async (CreateEscalationCommand command, IMediator mediator, CancellationToken ct) =>
            await mediator.Send(command, ct) ? Results.Created() : Results.BadRequest());
    }
}
