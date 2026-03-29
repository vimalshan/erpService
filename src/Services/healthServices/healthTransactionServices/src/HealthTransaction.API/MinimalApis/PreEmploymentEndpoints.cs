using HealthTransaction.Application.DTOs;
using HealthTransaction.Application.Features.PreEmploymentCheckups.Commands.Create;
using HealthTransaction.Application.Features.PreEmploymentCheckups.Queries.GetAll;
using HealthTransaction.Application.Features.PreEmploymentCheckups.Queries.GetByEmployeeNum;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace HealthTransaction.API.MinimalApis;

public static class PreEmploymentEndpoints
{
    public static void MapPreEmploymentEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/v2/pre-employment")
            .WithTags("PreEmployment v2")
            .RequireAuthorization();

        group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllPreEmploymentCheckupsQuery(), ct)));

        group.MapGet("/by-employee/{empNum}", async (decimal empNum, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetPreEmploymentCheckupsByEmployeeNumQuery(empNum), ct)));

        group.MapPost("/", async (CreatePreEmploymentCheckupDto dto, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new CreatePreEmploymentCheckupCommand(dto), ct);
            return Results.Created($"/api/v2/pre-employment/by-employee/{result.EmpNum}", result);
        });
    }
}
