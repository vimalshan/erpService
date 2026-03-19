using MediatR;
using MobileAppManagement.Application.Commands;
using MobileAppManagement.Application.Queries;

namespace MobileAppManagement.API.MinimalApis;

public static class RegistrationEndpoints
{
    public static void Map(WebApplication app)
    {
        var group = app.MapGroup("/api/minimal/registrations")
            .WithTags("Registrations (Minimal)")
            .RequireAuthorization();

        group.MapGet("/{registrationId}", async (long registrationId,
            IMediator mediator, CancellationToken ct) =>
        {
            try
            {
                var result = await mediator.Send(new GetRegistrationByIdQuery(registrationId), ct);
                return result is null ? Results.NotFound() : Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        });

        group.MapGet("/user/{userId}", async (string userId,
            IMediator mediator, CancellationToken ct) =>
        {
            try
            {
                var result = await mediator.Send(new GetRegistrationsByUserIdQuery(userId), ct);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        });

        group.MapGet("/status/{status}", async (string status,
            IMediator mediator, CancellationToken ct) =>
        {
            try
            {
                var result = await mediator.Send(new GetRegistrationsByStatusQuery(status), ct);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        });

        group.MapPost("/", async (CreateRegistrationCommand command,
            IMediator mediator, CancellationToken ct) =>
        {
            try
            {
                var result = await mediator.Send(command, ct);
                return Results.Created($"/api/minimal/registrations/{result.RegistrationId}", result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = "Registration creation failed" });
            }
        });

        group.MapPut("/{registrationId}/status", async (long registrationId,
            UpdateStatusInput input, IMediator mediator, CancellationToken ct) =>
        {
            try
            {
                var result = await mediator.Send(new UpdateRegistrationStatusCommand(registrationId, input.Status ?? ""), ct);
                return Results.Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = "Status update failed" });
            }
        });

        group.MapPost("/{registrationId}/generate-pin", async (long registrationId,
            IMediator mediator, CancellationToken ct) =>
        {
            try
            {
                var pin = await mediator.Send(new GenerateRegistrationPinCommand(registrationId), ct);
                return Results.Ok(new { pin });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = "PIN generation failed" });
            }
        });
    }
}

public record UpdateStatusInput(string? Status);
