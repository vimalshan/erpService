using MediatR;
using MobileAppManagement.Application.Commands;
using MobileAppManagement.Application.Queries;

namespace MobileAppManagement.API.MinimalApis;

public static class DeviceEndpoints
{
    public static void Map(WebApplication app)
    {
        var group = app.MapGroup("/api/minimal/devices")
            .WithTags("Devices (Minimal)")
            .RequireAuthorization();

        group.MapGet("/employee/{employeeSysId}", async (decimal employeeSysId,
            IMediator mediator, CancellationToken ct) =>
        {
            try
            {
                var result = await mediator.Send(new GetDevicesByEmployeeQuery(employeeSysId), ct);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        });

        group.MapGet("/{employeeSysId}/{deviceId}", async (decimal employeeSysId, string deviceId,
            IMediator mediator, CancellationToken ct) =>
        {
            try
            {
                var result = await mediator.Send(new GetDeviceByKeyQuery(employeeSysId, deviceId), ct);
                return result is null ? Results.NotFound() : Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        });

        group.MapPost("/register", async (RegisterDeviceCommand command,
            IMediator mediator, CancellationToken ct) =>
        {
            try
            {
                var result = await mediator.Send(command, ct);
                return Results.Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = "Device registration failed" });
            }
        });

        group.MapPost("/deactivate", async (DeactivateDeviceCommand command,
            IMediator mediator, CancellationToken ct) =>
        {
            try
            {
                var result = await mediator.Send(command, ct);
                return Results.Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = "Device deactivation failed" });
            }
        });
    }
}
