using MediatR;
using MedicineManagement.Application.DTOs;
using MedicineManagement.Application.Features.Medicines.Commands;
using MedicineManagement.Application.Features.Medicines.Queries;

namespace MedicineManagement.API.MinimalApis;

public static class MedicineEndpoints
{
    public static void Map(WebApplication app)
    {
        var group = app.MapGroup("/api/v2/medicines")
            .WithTags("Medicines (Minimal API)")
            .RequireAuthorization();

        group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllMedicinesQuery(), ct)))
            .AllowAnonymous();

        group.MapGet("/{medicineCode}", async (string medicineCode, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetMedicineByCodeQuery(medicineCode), ct);
            return result is not null ? Results.Ok(result) : Results.NotFound();
        }).AllowAnonymous();

        group.MapGet("/search", async (string name, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new SearchMedicinesQuery(name), ct)))
            .AllowAnonymous();

        group.MapPost("/", async (CreateMedicineDto dto, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new CreateMedicineCommand(
                dto.MedicineCode, dto.MedicineName, dto.MedicineTypeCode,
                dto.Category, dto.OrderLevelMin, dto.OrderLevelMax, "MinimalAPI", null), ct);
            return Results.Created($"/api/v2/medicines/{result.MedicineCode}", result);
        });

        group.MapPut("/{medicineCode}", async (string medicineCode, UpdateMedicineDto dto, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new UpdateMedicineCommand(
                medicineCode, dto.MedicineName, dto.MedicineTypeCode,
                dto.Category, dto.OrderLevelMin, dto.OrderLevelMax, "MinimalAPI", null), ct);
            return Results.Ok(result);
        });

        group.MapDelete("/{medicineCode}", async (string medicineCode, IMediator mediator, CancellationToken ct) =>
        {
            await mediator.Send(new DeleteMedicineCommand(medicineCode), ct);
            return Results.NoContent();
        });
    }
}
