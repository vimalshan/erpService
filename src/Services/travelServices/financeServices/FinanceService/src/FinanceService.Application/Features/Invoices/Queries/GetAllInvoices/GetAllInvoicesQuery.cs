using MediatR;
using FinanceService.Application.DTOs;

namespace FinanceService.Application.Features.Invoices.Queries.GetAllInvoices;

public record GetAllInvoicesQuery : IRequest<List<InvoiceDto>>;
