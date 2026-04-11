using MediatR;
using TransactionService.Application.DTOs;
using TransactionService.Application.EmployeeJournalVouchers.Queries;
using TransactionService.Application.SupplierJournalVouchers.Queries;
using TransactionService.Application.TravelBatches.Queries;
using TransactionService.Application.EmployeePayments;
using TransactionService.Application.AirlineInvoices;

namespace TransactionService.API.GraphQL;

public sealed class Query
{
    // ── Employee JV ──────────────────────────────────────────────────────────
    public async Task<IEnumerable<EmployeeJVDto>> GetEmployeeJVsAsync(
        [Service] IMediator mediator,
        int page = 1, int pageSize = 20,
        long? employeeId = null, string? status = null,
        CancellationToken ct = default)
        => await mediator.Send(new GetAllEmployeeJVsQuery(page, pageSize, employeeId, status), ct);

    public async Task<EmployeeJVDto> GetEmployeeJVAsync(
        [Service] IMediator mediator, long jvBatchId, CancellationToken ct)
        => await mediator.Send(new GetEmployeeJVByIdQuery(jvBatchId), ct);

    // ── Supplier JV ──────────────────────────────────────────────────────────
    public async Task<IEnumerable<SupplierJVDto>> GetSupplierJVsAsync(
        [Service] IMediator mediator,
        int page = 1, int pageSize = 20, long? vendorId = null,
        CancellationToken ct = default)
        => await mediator.Send(new GetAllSupplierJVsQuery(page, pageSize, vendorId), ct);

    public async Task<SupplierJVDto> GetSupplierJVAsync(
        [Service] IMediator mediator, long jvId, CancellationToken ct)
        => await mediator.Send(new GetSupplierJVByIdQuery(jvId), ct);

    // ── Travel Batch ─────────────────────────────────────────────────────────
    public async Task<IEnumerable<TravelBatchDto>> GetTravelBatchesAsync(
        [Service] IMediator mediator,
        int page = 1, int pageSize = 20,
        string? status = null, string? vendorId = null,
        CancellationToken ct = default)
        => await mediator.Send(new GetAllTravelBatchesQuery(page, pageSize, status, vendorId), ct);

    public async Task<TravelBatchDto> GetTravelBatchAsync(
        [Service] IMediator mediator, string batchId, CancellationToken ct)
        => await mediator.Send(new GetTravelBatchByIdQuery(batchId), ct);

    // ── Employee Payment ─────────────────────────────────────────────────────
    public async Task<EmployeePaymentDto> GetEmployeePaymentAsync(
        [Service] IMediator mediator, long payId, CancellationToken ct)
        => await mediator.Send(new GetEmployeePaymentByIdQuery(payId), ct);

    public async Task<IEnumerable<EmployeePaymentDto>> GetEmployeePaymentsByEmployeeAsync(
        [Service] IMediator mediator, long empSysId, CancellationToken ct)
        => await mediator.Send(new GetEmployeePaymentsByEmployeeQuery(empSysId), ct);

    // ── Airline Invoice ──────────────────────────────────────────────────────
    public async Task<AirlineInvoiceDto> GetAirlineInvoiceAsync(
        [Service] IMediator mediator, string airTicketId, CancellationToken ct)
        => await mediator.Send(new GetAirlineInvoiceByIdQuery(airTicketId), ct);

    public async Task<IEnumerable<AirlineInvoiceDto>> GetAirlineInvoicesByBookingAsync(
        [Service] IMediator mediator, string bookCnfId, CancellationToken ct)
        => await mediator.Send(new GetAirlineInvoicesByBookingQuery(bookCnfId), ct);
}
