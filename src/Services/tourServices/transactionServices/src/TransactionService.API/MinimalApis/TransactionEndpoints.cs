using MediatR;
using TransactionService.Application.EmployeeJournalVouchers.Queries;
using TransactionService.Application.SupplierJournalVouchers.Queries;
using TransactionService.Application.TravelBatches.Queries;
using TransactionService.Application.EmployeePayments;
using TransactionService.Application.AirlineInvoices;

namespace TransactionService.API.MinimalApis;

public static class TransactionEndpoints
{
    public static void MapTransactionEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/v2/transactions")
            .WithTags("Transactions (Minimal API)")
            .RequireAuthorization();

        group.MapGet("/employee-jvs", async (IMediator mediator, int page = 1, int pageSize = 20, CancellationToken ct = default) =>
            Results.Ok(await mediator.Send(new GetAllEmployeeJVsQuery(page, pageSize), ct)))
            .WithName("GetAllEmployeeJVsV2");

        group.MapGet("/employee-jvs/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetEmployeeJVByIdQuery(id), ct)))
            .WithName("GetEmployeeJVByIdV2");

        group.MapGet("/supplier-jvs", async (IMediator mediator, int page = 1, int pageSize = 20, CancellationToken ct = default) =>
            Results.Ok(await mediator.Send(new GetAllSupplierJVsQuery(page, pageSize), ct)))
            .WithName("GetAllSupplierJVsV2");

        group.MapGet("/supplier-jvs/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetSupplierJVByIdQuery(id), ct)))
            .WithName("GetSupplierJVByIdV2");

        group.MapGet("/travel-batches", async (IMediator mediator, int page = 1, int pageSize = 20, string? status = null, CancellationToken ct = default) =>
            Results.Ok(await mediator.Send(new GetAllTravelBatchesQuery(page, pageSize, status), ct)))
            .WithName("GetAllTravelBatchesV2");

        group.MapGet("/travel-batches/{id}", async (string id, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetTravelBatchByIdQuery(id), ct)))
            .WithName("GetTravelBatchByIdV2");

        group.MapGet("/employee-payments/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetEmployeePaymentByIdQuery(id), ct)))
            .WithName("GetEmployeePaymentByIdV2");

        group.MapGet("/airline-invoices/{id}", async (string id, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAirlineInvoiceByIdQuery(id), ct)))
            .WithName("GetAirlineInvoiceByIdV2");
    }
}
