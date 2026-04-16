using FinanceService.Application.DTOs;
using MediatR;

namespace FinanceService.Application.Commands;

public record CreateInvoiceCommand(CreateInvoiceDto Dto) : IRequest<InvoiceDto>;
public record UpdateInvoiceCommand(UpdateInvoiceDto Dto) : IRequest<InvoiceDto>;
public record DeleteInvoiceCommand(int InvoiceId) : IRequest<bool>;
public record MarkInvoicePaidCommand(int InvoiceId, DateTime PaidDate, string? PaymentMethod, string? PaymentReference, int? ModifiedBy) : IRequest<InvoiceDto>;
public record ChangeInvoiceStatusCommand(int InvoiceId, string NewStatus, int? ModifiedBy) : IRequest<InvoiceDto>;
public record CreateFinancialCommand(CreateFinancialDto Dto) : IRequest<FinancialDto>;
