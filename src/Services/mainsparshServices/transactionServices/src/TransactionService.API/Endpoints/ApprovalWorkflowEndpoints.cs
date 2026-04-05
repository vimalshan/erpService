using MediatR;
using TransactionService.Application.DTOs;
using TransactionService.Application.Features.ApprovalWorkflows.Commands;
using TransactionService.Application.Features.ApprovalWorkflows.Queries;

namespace TransactionService.API.Endpoints;

public static class ApprovalWorkflowEndpoints
{
    public static WebApplication MapApprovalWorkflowEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/minimal/workflows").WithTags("Minimal - Workflows");

        group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAllWorkflowsQuery(), ct);
            return Results.Ok(result);
        }).RequireAuthorization();

        group.MapGet("/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetWorkflowByIdQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).RequireAuthorization();

        group.MapGet("/pending/{approverId:long}", async (long approverId, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetPendingWorkflowsQuery(approverId), ct);
            return Results.Ok(result);
        }).RequireAuthorization();

        group.MapPost("/", async (SubmitWorkflowCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/minimal/workflows/{result.WorkflowId}", result);
        }).RequireAuthorization();

        group.MapPost("/{id:long}/approve", async (long id, ApproveStepCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Ok(result);
        }).RequireAuthorization();

        group.MapPost("/{id:long}/reject", async (long id, RejectStepCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Ok(result);
        }).RequireAuthorization();

        return app;
    }
}
