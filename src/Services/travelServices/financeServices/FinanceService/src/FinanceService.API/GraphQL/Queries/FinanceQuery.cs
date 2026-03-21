using FinanceService.Application.Common.Interfaces;
using FinanceService.Application.DTOs;
using FinanceService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceService.API.GraphQL.Queries;

public class FinanceQuery
{
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<ApInvoice> GetInvoices([Service] IFinanceDbContext context)
        => context.ApInvoices.Include(i => i.InvoiceLines);

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<TravelBatchMain> GetBatches([Service] IFinanceDbContext context)
        => context.TravelBatchMains.Include(b => b.BatchLines);

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<TravelAccount> GetPayments([Service] IFinanceDbContext context)
        => context.TravelAccounts;

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<PaymentTerm> GetPaymentTerms([Service] IFinanceDbContext context)
        => context.PaymentTerms;

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<JvPostingDetail> GetJvPostings([Service] IFinanceDbContext context)
        => context.JvPostingDetails;
}
