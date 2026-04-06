using Microsoft.EntityFrameworkCore;
using PayTransactionalService.Domain.Common;
using PayTransactionalService.Domain.Entities;

namespace PayTransactionalService.Infrastructure.Persistence;

public class PayTransactionalDbContext : DbContext
{
    public PayTransactionalDbContext(DbContextOptions<PayTransactionalDbContext> options) : base(options) { }

    public DbSet<PayTransaction> PayTransactions { get; set; } = null!;
    public DbSet<PayArrear> PayArrears { get; set; } = null!;
    public DbSet<PayAdjustment> PayAdjustments { get; set; } = null!;
    public DbSet<PayrollBatch> PayrollBatches { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Ignore domain events
        modelBuilder.Ignore<DomainEvent>();
        modelBuilder.Ignore<PayTransactionCreatedEvent>();
        modelBuilder.Ignore<PayTransactionCompletedEvent>();
        modelBuilder.Ignore<PayTransactionRevokedEvent>();
        modelBuilder.Ignore<PayArrearCreatedEvent>();
        modelBuilder.Ignore<PayAdjustmentCreatedEvent>();
        modelBuilder.Ignore<PayAdjustmentApprovedEvent>();
        modelBuilder.Ignore<PayAdjustmentRejectedEvent>();
        modelBuilder.Ignore<PayrollBatchCreatedEvent>();
        modelBuilder.Ignore<PayrollBatchCompletedEvent>();

        // PayTransaction (PAY_TRANDET)
        modelBuilder.Entity<PayTransaction>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.EmployeeSystemId).IsRequired();
            entity.Property(e => e.MonthYear).IsRequired().HasMaxLength(7);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(1).HasDefaultValue("P");
            entity.Property(e => e.Remarks).HasMaxLength(500);

            entity.OwnsOne(e => e.GrossAmount, nav =>
            {
                nav.Property(m => m.Amount).HasColumnName("GrossAmount").HasPrecision(19, 2);
                nav.Property(m => m.Currency).HasColumnName("GrossAmountCurrency").HasMaxLength(3).HasDefaultValue("INR");
            });
            entity.OwnsOne(e => e.Deductions, nav =>
            {
                nav.Property(m => m.Amount).HasColumnName("Deductions").HasPrecision(19, 2);
                nav.Property(m => m.Currency).HasColumnName("DeductionsCurrency").HasMaxLength(3).HasDefaultValue("INR");
            });
            entity.OwnsOne(e => e.NetAmount, nav =>
            {
                nav.Property(m => m.Amount).HasColumnName("NetAmount").HasPrecision(19, 2);
                nav.Property(m => m.Currency).HasColumnName("NetAmountCurrency").HasMaxLength(3).HasDefaultValue("INR");
            });

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);

            entity.HasIndex(e => new { e.EmployeeSystemId, e.MonthYear }).HasDatabaseName("IX_PayTran_Emp_Month");
            entity.HasIndex(e => e.BatchId).HasDatabaseName("IX_PayTran_BatchId");
            entity.HasIndex(e => e.Status).HasDatabaseName("IX_PayTran_Status");
        });

        // PayArrear (PAY_ARR)
        modelBuilder.Entity<PayArrear>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.EmployeeSystemId).IsRequired();
            entity.Property(e => e.Type).IsRequired().HasMaxLength(1);
            entity.Property(e => e.Code).HasMaxLength(20);
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.MonthYear).IsRequired().HasMaxLength(7);
            entity.Property(e => e.IsProcessed).HasDefaultValue(false);

            entity.OwnsOne(e => e.Amount, nav =>
            {
                nav.Property(m => m.Amount).HasColumnName("Amount").HasPrecision(19, 2);
                nav.Property(m => m.Currency).HasColumnName("AmountCurrency").HasMaxLength(3).HasDefaultValue("INR");
            });

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);

            entity.HasIndex(e => new { e.EmployeeSystemId, e.MonthYear }).HasDatabaseName("IX_PayArr_Emp_Month");
            entity.HasIndex(e => e.Type).HasDatabaseName("IX_PayArr_Type");
        });

        // PayAdjustment (PAY_ADJWRK)
        modelBuilder.Entity<PayAdjustment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.EmployeeSystemId).IsRequired();
            entity.Property(e => e.AdjustmentType).IsRequired().HasMaxLength(20);
            entity.Property(e => e.MonthYear).IsRequired().HasMaxLength(7);
            entity.Property(e => e.Reason).HasMaxLength(500);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(1).HasDefaultValue("P");

            entity.OwnsOne(e => e.Amount, nav =>
            {
                nav.Property(m => m.Amount).HasColumnName("Amount").HasPrecision(19, 2);
                nav.Property(m => m.Currency).HasColumnName("AmountCurrency").HasMaxLength(3).HasDefaultValue("INR");
            });

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);

            entity.HasIndex(e => new { e.EmployeeSystemId, e.MonthYear }).HasDatabaseName("IX_PayAdj_Emp_Month");
            entity.HasIndex(e => e.Status).HasDatabaseName("IX_PayAdj_Status");
        });

        // PayrollBatch
        modelBuilder.Entity<PayrollBatch>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.MonthYear).IsRequired().HasMaxLength(7);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(1).HasDefaultValue("P");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);

            entity.HasIndex(e => e.MonthYear).IsUnique().HasDatabaseName("IX_Batch_MonthYear");
        });
    }
}
