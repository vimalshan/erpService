using FinanceService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceService.Infrastructure.Data;

public class FinanceDomainDbContext : DbContext
{
    public FinanceDomainDbContext(DbContextOptions<FinanceDomainDbContext> options) : base(options) { }

    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<InvoiceAuditLog> InvoiceAuditLogs => Set<InvoiceAuditLog>();
    public DbSet<Financial> Financials => Set<Financial>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Invoice>(e =>
        {
            e.ToTable("Invoices"); e.HasKey(x => x.InvoiceId);
            e.Property(x => x.InvoiceNumber).HasMaxLength(50).IsRequired();
            e.Property(x => x.Amount).HasColumnType("decimal(12,2)");
            e.Property(x => x.TaxAmount).HasColumnType("decimal(12,2)").HasDefaultValue(0m);
            e.Property(x => x.TotalAmount).HasColumnType("decimal(12,2)");
            e.Property(x => x.Currency).HasMaxLength(3).HasDefaultValue("USD");
            e.Property(x => x.Status).HasMaxLength(50).HasDefaultValue("Pending");
            e.Property(x => x.IsActive).HasDefaultValue(true);
            e.Property(x => x.CreatedDate).HasDefaultValueSql("GETDATE()");
            e.Property(x => x.ModifiedDate).HasDefaultValueSql("GETDATE()");
            e.Property(x => x.Description).HasMaxLength(500);
            e.Property(x => x.Terms).HasMaxLength(1000);
            e.Property(x => x.Notes).HasMaxLength(1000);
            e.Property(x => x.InvoicePath).HasMaxLength(500);
            e.Property(x => x.PaymentMethod).HasMaxLength(50);
            e.Property(x => x.PaymentReference).HasMaxLength(100);
            e.Property(x => x.DiscountAmount).HasColumnType("decimal(10,2)").HasDefaultValue(0m);
            e.Property(x => x.LateFee).HasColumnType("decimal(10,2)").HasDefaultValue(0m);
            e.HasIndex(x => x.InvoiceNumber).IsUnique();
            e.HasIndex(x => x.CompanyId); e.HasIndex(x => x.Status);
            e.HasIndex(x => x.DueDate); e.HasIndex(x => x.InvoiceDate); e.HasIndex(x => x.IsActive);
            e.Ignore(x => x.DomainEvents);
            e.HasMany(x => x.AuditLogs).WithOne(x => x.Invoice).HasForeignKey(x => x.InvoiceId);
        });

        modelBuilder.Entity<InvoiceAuditLog>(e =>
        {
            e.ToTable("InvoiceAuditLog"); e.HasKey(x => x.InvoiceAuditLogId);
            e.Property(x => x.InvoiceNumber).HasMaxLength(50).IsRequired();
            e.Property(x => x.Action).HasMaxLength(50).IsRequired();
            e.Property(x => x.ChangedFields).HasMaxLength(500);
            e.Property(x => x.Reason).HasMaxLength(500);
            e.Property(x => x.ActionDate).HasDefaultValueSql("GETDATE()");
            e.Property(x => x.IPAddress).HasMaxLength(45);
            e.Property(x => x.UserAgent).HasMaxLength(500);
            e.HasIndex(x => x.InvoiceId); e.HasIndex(x => x.ActionDate); e.HasIndex(x => x.ActionBy);
        });

        modelBuilder.Entity<Financial>(e =>
        {
            e.ToTable("Financials"); e.HasKey(x => x.FinancialId);
            e.Property(x => x.Revenue).HasColumnType("decimal(15,2)");
            e.Property(x => x.Expenses).HasColumnType("decimal(15,2)");
            e.Property(x => x.Profit).HasColumnType("decimal(15,2)");
            e.Property(x => x.OutstandingAmount).HasColumnType("decimal(15,2)");
            e.Property(x => x.PaidAmount).HasColumnType("decimal(15,2)");
            e.Property(x => x.OverdueAmount).HasColumnType("decimal(15,2)");
            e.Property(x => x.Currency).HasMaxLength(3).HasDefaultValue("USD");
            e.Property(x => x.IsActive).HasDefaultValue(true);
            e.Property(x => x.CreatedDate).HasDefaultValueSql("GETDATE()");
            e.Property(x => x.ModifiedDate).HasDefaultValueSql("GETDATE()");
            e.Property(x => x.Notes).HasMaxLength(1000);
            e.Property(x => x.DataSource).HasMaxLength(100);
            e.HasIndex(x => x.CompanyId); e.HasIndex(x => x.Year); e.HasIndex(x => x.IsActive);
            e.HasIndex(x => new { x.CompanyId, x.Year, x.Quarter, x.Month }).IsUnique();
        });
    }
}
