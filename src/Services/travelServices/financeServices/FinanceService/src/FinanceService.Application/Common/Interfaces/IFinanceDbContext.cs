using FinanceService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceService.Application.Common.Interfaces;

public interface IFinanceDbContext
{
    DbSet<ApInvoice> ApInvoices { get; }
    DbSet<ApInvoiceLine> ApInvoiceLines { get; }
    DbSet<PaymentTerm> PaymentTerms { get; }
    DbSet<PaymentDetail> PaymentDetails { get; }
    DbSet<TravelBatchMain> TravelBatchMains { get; }
    DbSet<TravelBatchSub> TravelBatchSubs { get; }
    DbSet<JvPostingDetail> JvPostingDetails { get; }
    DbSet<PayJv> PayJvs { get; }
    DbSet<PayOtherDetail> PayOtherDetails { get; }
    DbSet<TravelAccount> TravelAccounts { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
