using Microsoft.EntityFrameworkCore;
using TransactionService.Application.Common.Interfaces;
using TransactionService.Domain.Aggregates;
using TransactionService.Domain.Entities;

namespace TransactionService.Infrastructure.Persistence;

public sealed class TransactionDbContext(DbContextOptions<TransactionDbContext> options)
    : DbContext(options), IApplicationDbContext
{
    public DbSet<EmployeeJournalVoucher> EmployeeJournalVouchers => Set<EmployeeJournalVoucher>();
    public DbSet<EmployeeJVLine> EmployeeJVLines => Set<EmployeeJVLine>();
    public DbSet<SupplierJournalVoucher> SupplierJournalVouchers => Set<SupplierJournalVoucher>();
    public DbSet<SupplierJVLine> SupplierJVLines => Set<SupplierJVLine>();
    public DbSet<TravelBatch> TravelBatches => Set<TravelBatch>();
    public DbSet<TravelBatchSub> TravelBatchSubs => Set<TravelBatchSub>();
    public DbSet<TravelBatchCostCentre> TravelBatchCostCentres => Set<TravelBatchCostCentre>();
    public DbSet<TravelBatchContract> TravelBatchContracts => Set<TravelBatchContract>();
    public DbSet<TravelBatchSubBreak> TravelBatchSubBreaks => Set<TravelBatchSubBreak>();
    public DbSet<EmployeePayment> EmployeePayments => Set<EmployeePayment>();
    public DbSet<EmployeeTravelPay> EmployeeTravelPays => Set<EmployeeTravelPay>();
    public DbSet<AirlineInvoice> AirlineInvoices => Set<AirlineInvoice>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TransactionDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
