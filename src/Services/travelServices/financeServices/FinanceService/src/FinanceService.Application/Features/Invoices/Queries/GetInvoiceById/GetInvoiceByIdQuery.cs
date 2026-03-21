using MediatR;
using FinanceService.Application.DTOs;

namespace FinanceService.Application.Features.Invoices.Queries.GetInvoiceById;

public record GetInvoiceByIdQuery(long InvoiceId) : IRequest<InvoiceDto>;
