using EmployeePrideManagement.Application.Commands.CreatePrideMoment;
using EmployeePrideManagement.Application.Commands.DeletePrideMoment;
using EmployeePrideManagement.Application.Commands.UpdatePrideMoment;
using EmployeePrideManagement.Application.DTOs;
using EmployeePrideManagement.Application.Queries.GetAllPrideMoments;
using EmployeePrideManagement.Application.Queries.GetPrideMomentById;
using EmployeePrideManagement.Application.Queries.GetPrideMomentsByEmployee;
using MediatR;

namespace EmployeePrideManagement.API.Endpoints;

public static class PrideMomentEndpoints
{
    public static void MapPrideMomentEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v2/pride-moments")
            .WithTags("PrideMoments-MinimalAPI")
            .RequireAuthorization();

        group.MapGet("/", async (IMediator mediator, int pageNumber = 1, int pageSize = 10) =>
        {
            var result = await mediator.Send(new GetAllPrideMomentsQuery(pageNumber, pageSize));
            return Results.Ok(result);
        })
        .WithName("GetAllPrideMomentsV2")
        .Produces<PagedResultDto<PrideMomentDto>>();

        group.MapGet("/{id:decimal}", async (IMediator mediator, decimal id) =>
        {
            var result = await mediator.Send(new GetPrideMomentByIdQuery(id));
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("GetPrideMomentByIdV2")
        .Produces<PrideMomentDto>()
        .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/employee/{employeeSysId:decimal}", async (IMediator mediator, decimal employeeSysId) =>
        {
            var result = await mediator.Send(new GetPrideMomentsByEmployeeQuery(employeeSysId));
            return Results.Ok(result);
        })
        .WithName("GetPrideMomentsByEmployeeV2")
        .Produces<IEnumerable<PrideMomentDto>>();

        group.MapPost("/", async (IMediator mediator, CreatePrideMomentDto dto) =>
        {
            var command = new CreatePrideMomentCommand(
                dto.Title, dto.Body, dto.EmployeeSysId,
                dto.Footer, dto.Location, dto.ImagePath, dto.ModifiedBy);

            var result = await mediator.Send(command);
            return Results.Created($"/api/v2/pride-moments/{result.MomentPrideId}", result);
        })
        .WithName("CreatePrideMomentV2")
        .Produces<PrideMomentDto>(StatusCodes.Status201Created);

        group.MapPut("/{id:decimal}", async (IMediator mediator, decimal id, UpdatePrideMomentDto dto) =>
        {
            var command = new UpdatePrideMomentCommand(
                id, dto.Title, dto.Body, dto.Footer,
                dto.Location, dto.ImagePath, dto.ModifiedBy);

            var result = await mediator.Send(command);
            return Results.Ok(result);
        })
        .WithName("UpdatePrideMomentV2")
        .Produces<PrideMomentDto>();

        group.MapDelete("/{id:decimal}", async (IMediator mediator, decimal id) =>
        {
            await mediator.Send(new DeletePrideMomentCommand(id));
            return Results.NoContent();
        })
        .WithName("DeletePrideMomentV2")
        .Produces(StatusCodes.Status204NoContent);
    }
}
