using CustomerService.Application.Features.Customers.Commands;
using CustomerService.Application.Features.Customers.Queries;
using MediatR;

namespace CustomerService.API.MinimalApis;

public static class CustomerEndpoints
{
    public static void MapCustomerEndpoints(WebApplication app)
    {
        var group = app.MapGroup("/api/v2/customers")
            .WithTags("Customers (Minimal API)")
            .RequireAuthorization();

        group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAllCustomersQuery(), ct);
            return Results.Ok(result);
        }).WithName("GetAllCustomersMinimal");

        group.MapGet("/{id:int}", async (int id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetCustomerByIdQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetCustomerByIdMinimal");

        group.MapGet("/paged", async (int page, int pageSize, string? search, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetCustomersPagedQuery(page, pageSize, search), ct);
            return Results.Ok(result);
        }).WithName("GetCustomersPagedMinimal");

        group.MapPost("/", async (CreateCustomerCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/customers/{result.CustomerId}", result);
        }).WithName("CreateCustomerMinimal");

        group.MapPut("/{id:int}", async (int id, UpdateCustomerCommand command, IMediator mediator, CancellationToken ct) =>
        {
            if (id != command.CustomerId)
                return Results.BadRequest("ID mismatch.");
            var result = await mediator.Send(command, ct);
            return Results.Ok(result);
        }).WithName("UpdateCustomerMinimal");

        group.MapDelete("/{id:int}", async (int id, IMediator mediator, CancellationToken ct) =>
        {
            await mediator.Send(new DeleteCustomerCommand(id), ct);
            return Results.NoContent();
        }).WithName("DeleteCustomerMinimal");
    }
}
