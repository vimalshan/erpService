using MediatR;
using ReimbursementService.Application.DTOs;
using ReimbursementService.Application.Features.Reimbursements.Commands.CreateReimbursement;
using ReimbursementService.Application.Features.Reimbursements.Queries.GetAllReimbursements;
using ReimbursementService.Application.Features.Reimbursements.Queries.GetReimbursementById;
using ReimbursementService.Application.Features.Reimbursements.Queries.GetReimbursementsByEmployee;

namespace ReimbursementService.API.MinimalApis;

/// <summary>Minimal API endpoints supplementing the REST controllers.</summary>
public static class ReimbursementEndpoints
{
    public static IEndpointRouteBuilder MapReimbursementEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/reimbursements")
            .WithTags("Reimbursements (Minimal API)")
            .RequireAuthorization();

        group.MapGet("/", async (ISender mediator, int page = 1, int size = 20, CancellationToken ct = default) =>
        {
            var result = await mediator.Send(new GetAllReimbursementsQuery(page, size), ct);
            return Results.Ok(result);
        })
        .WithName("MinimalGetAll")
        .WithSummary("Get all reimbursements (paginated)");

        group.MapGet("/{id:long}", async (long id, ISender mediator, CancellationToken ct = default) =>
        {
            var result = await mediator.Send(new GetReimbursementByIdQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("MinimalGetById")
        .WithSummary("Get a reimbursement by ID");

        group.MapGet("/employee/{empSysId:long}", async (long empSysId, ISender mediator, CancellationToken ct = default) =>
        {
            var result = await mediator.Send(new GetReimbursementsByEmployeeQuery(empSysId), ct);
            return Results.Ok(result);
        })
        .WithName("MinimalGetByEmployee")
        .WithSummary("Get reimbursements for an employee");

        group.MapPost("/", async (CreateReimbursementRequestDto dto, ISender mediator, CancellationToken ct = default) =>
        {
            var command = new CreateReimbursementCommand(
                dto.EmpSysId, dto.ReimType, dto.Amount, dto.Currency,
                dto.ReimDate, dto.ExpenseDate, dto.Description, dto.Location, dto.EmpSysId);
            var result = await mediator.Send(command, ct);
            return Results.Created($"/reimbursements/{result.ReimId}", result);
        })
        .WithName("MinimalCreate")
        .WithSummary("Create a new reimbursement");

        return app;
    }
}
