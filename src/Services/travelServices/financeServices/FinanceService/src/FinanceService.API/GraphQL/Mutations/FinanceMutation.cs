using FinanceService.Application.DTOs;
using FinanceService.Application.Features.Batches.Commands.ApproveBatch;
using FinanceService.Application.Features.Batches.Commands.CreateBatch;
using FinanceService.Application.Features.Invoices.Commands.CreateInvoice;
using FinanceService.Application.Features.Payments.Commands.ProcessPayment;
using MediatR;

namespace FinanceService.API.GraphQL.Mutations;

public class FinanceMutation
{
    public async Task<InvoiceDto> CreateInvoice(
        [Service] IMediator mediator,
        CreateInvoiceCommand input,
        CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<BatchDto> CreateBatch(
        [Service] IMediator mediator,
        CreateBatchCommand input,
        CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<bool> ApproveBatch(
        [Service] IMediator mediator,
        ApproveBatchCommand input,
        CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<PaymentDto> ProcessPayment(
        [Service] IMediator mediator,
        ProcessPaymentCommand input,
        CancellationToken ct)
        => await mediator.Send(input, ct);
}
