using FinanceService.Application.Commands;
using FinanceService.Application.DTOs;
using FinanceService.Models;
using FinanceService.Services;
using MediatR;

namespace FinanceService.GraphQL.Mutations
{
    public class Mutation
    {
        private readonly IFinanceService _service;

        public Mutation(IFinanceService service)
        {
            _service = service;
        }

        [GraphQLName("UpdatePlannedPaymentDate")]
        public Task<ApiResponse<bool>> UpdatePlannedPaymentDate(List<string> invoiceNumber, DateTime plannedDates)
        {
            return _service.UpdatePlannedPaymentDateAsync(invoiceNumber, plannedDates);
        }

        [GraphQLName("createInvoice")]
        public async Task<InvoiceDto> CreateInvoice([Service] IMediator mediator, CreateInvoiceDto input)
            => await mediator.Send(new CreateInvoiceCommand(input));

        [GraphQLName("updateInvoice")]
        public async Task<InvoiceDto> UpdateInvoice([Service] IMediator mediator, UpdateInvoiceDto input)
            => await mediator.Send(new UpdateInvoiceCommand(input));

        [GraphQLName("deleteInvoice")]
        public async Task<bool> DeleteInvoice([Service] IMediator mediator, int invoiceId)
            => await mediator.Send(new DeleteInvoiceCommand(invoiceId));

        [GraphQLName("markInvoicePaid")]
        public async Task<InvoiceDto> MarkInvoicePaid([Service] IMediator mediator, int invoiceId, DateTime paidDate, string? paymentMethod, string? paymentReference, int? modifiedBy)
            => await mediator.Send(new MarkInvoicePaidCommand(invoiceId, paidDate, paymentMethod, paymentReference, modifiedBy));

        [GraphQLName("changeInvoiceStatus")]
        public async Task<InvoiceDto> ChangeInvoiceStatus([Service] IMediator mediator, int invoiceId, string newStatus, int? modifiedBy)
            => await mediator.Send(new ChangeInvoiceStatusCommand(invoiceId, newStatus, modifiedBy));

        [GraphQLName("createFinancial")]
        public async Task<FinancialDto> CreateFinancial([Service] IMediator mediator, CreateFinancialDto input)
            => await mediator.Send(new CreateFinancialCommand(input));
    }
}
