using MediatR;
using MedicineManagement.Application.Features.MedicineCredits.Queries;

namespace MedicineManagement.API.MinimalApis;

public static class StockEndpoints
{
    public static void Map(WebApplication app)
    {
        var group = app.MapGroup("/api/v2/stock")
            .WithTags("Stock (Minimal API)")
            .RequireAuthorization();

        group.MapGet("/balance/{medicineCode}", async (string medicineCode, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetStockByMedicineQuery(medicineCode), ct)));

        group.MapGet("/transactions/{medicineCode}", async (string medicineCode, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetTransactionsByMedicineQuery(medicineCode), ct)));

        group.MapGet("/transactions/by-date", async (DateTime from, DateTime to, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetTransactionsByDateRangeQuery(from, to), ct)));
    }
}
