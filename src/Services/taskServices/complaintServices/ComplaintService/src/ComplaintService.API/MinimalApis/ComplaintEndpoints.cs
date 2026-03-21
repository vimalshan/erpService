using ComplaintService.Application.Commands.CloseComplaint;
using ComplaintService.Application.Commands.CreateComplaint;
using ComplaintService.Application.Commands.ReopenComplaint;
using ComplaintService.Application.DTOs;
using ComplaintService.Application.Queries.GetAllComplaints;
using ComplaintService.Application.Queries.GetComplaintById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ComplaintService.API.MinimalApis;

public static class ComplaintEndpoints
{
    public static WebApplication MapComplaintEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/minimal/complaints")
            .WithTags("Complaints-Minimal")
            .RequireAuthorization();

        group.MapGet("/", async ([FromServices] ISender mediator,
            [FromQuery] int page = 1, [FromQuery] int size = 20,
            CancellationToken ct = default) =>
        {
            var result = await mediator.Send(new GetAllComplaintsQuery(page, size), ct);
            return Results.Ok(result);
        })
        .WithName("MinimalGetAllComplaints")
        .Produces<IEnumerable<ComplaintTicketDto>>();

        group.MapGet("/{ticketNum:decimal}", async (decimal ticketNum,
            [FromServices] ISender mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetComplaintByIdQuery(ticketNum), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("MinimalGetComplaintById")
        .Produces<ComplaintTicketDto>()
        .Produces(404);

        group.MapPost("/", async ([FromBody] CreateComplaintRequest request,
            [FromServices] ISender mediator, HttpContext ctx, CancellationToken ct) =>
        {
            decimal.TryParse(ctx.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value, out var userId);
            var ticketNum = await mediator.Send(new CreateComplaintCommand(
                request.GroupId, request.Type, request.Location, request.Department,
                request.Process, request.Subject, request.Description, request.IsNCR,
                request.TargetResolutionHours, userId), ct);
            return Results.Created($"/api/minimal/complaints/{ticketNum}", ticketNum);
        })
        .WithName("MinimalCreateComplaint")
        .Produces<decimal>(201)
        .Produces(400);

        return app;
    }
}
