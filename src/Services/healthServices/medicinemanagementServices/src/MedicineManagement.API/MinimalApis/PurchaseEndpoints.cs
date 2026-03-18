using MediatR;
using MedicineManagement.Application.DTOs;
using MedicineManagement.Application.Features.Purchases.Commands;
using MedicineManagement.Application.Features.Purchases.Queries;

namespace MedicineManagement.API.MinimalApis;

public static class PurchaseEndpoints
{
    public static void Map(WebApplication app)
    {
        var group = app.MapGroup("/api/v2/purchases")
            .WithTags("Purchases (Minimal API)")
            .RequireAuthorization();

        group.MapGet("/{companyCode}/{transactionNumber}", async (string companyCode, long transactionNumber, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetPurchaseByIdQuery(companyCode, transactionNumber), ct);
            return result is not null ? Results.Ok(result) : Results.NotFound();
        });

        group.MapGet("/by-date", async (DateTime from, DateTime to, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetPurchasesByDateRangeQuery(from, to), ct)));

        group.MapPost("/", async (CreatePurchaseDto dto, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new CreatePurchaseCommand(
                dto.CompanyCode, dto.TransactionNumber, dto.VendorName,
                dto.InvoiceNumber, dto.InvoiceDate, dto.InvoiceAmount,
                "MinimalAPI", 0, dto.LineItems), ct);
            return Results.Created($"/api/v2/purchases/{result.CompanyCode}/{result.TransactionNumber}", result);
        });

        group.MapPost("/{companyCode}/{transactionNumber}/cancel", async (string companyCode, long transactionNumber, IMediator mediator, CancellationToken ct) =>
        {
            await mediator.Send(new CancelPurchaseCommand(companyCode, transactionNumber, "MinimalAPI", 0), ct);
            return Results.NoContent();
        });
    }
}
