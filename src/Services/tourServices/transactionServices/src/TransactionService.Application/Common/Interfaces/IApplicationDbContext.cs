using Microsoft.EntityFrameworkCore;
using TransactionService.Domain.Aggregates;
using TransactionService.Domain.Entities;

namespace TransactionService.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<EmployeeJournalVoucher> EmployeeJournalVouchers { get; }
    DbSet<EmployeeJVLine> EmployeeJVLines { get; }
    DbSet<SupplierJournalVoucher> SupplierJournalVouchers { get; }
    DbSet<SupplierJVLine> SupplierJVLines { get; }
    DbSet<TravelBatch> TravelBatches { get; }
    DbSet<TravelBatchSub> TravelBatchSubs { get; }
    DbSet<TravelBatchCostCentre> TravelBatchCostCentres { get; }
    DbSet<TravelBatchContract> TravelBatchContracts { get; }
    DbSet<TravelBatchSubBreak> TravelBatchSubBreaks { get; }
    DbSet<EmployeePayment> EmployeePayments { get; }
    DbSet<EmployeeTravelPay> EmployeeTravelPays { get; }
    DbSet<AirlineInvoice> AirlineInvoices { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
