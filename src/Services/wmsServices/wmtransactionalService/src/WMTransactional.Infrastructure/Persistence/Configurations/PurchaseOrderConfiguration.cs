using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMTransactional.Domain.Entities;

namespace WMTransactional.Infrastructure.Persistence.Configurations;

public class PurchaseOrderConfiguration : IEntityTypeConfiguration<PurchaseOrder>
{
    public void Configure(EntityTypeBuilder<PurchaseOrder> builder)
    {
        builder.ToTable("PurchaseOrder");
        builder.HasKey(p => p.PoId);
        builder.Property(p => p.PoId).HasColumnName("po_id").ValueGeneratedOnAdd();
        builder.Property(p => p.PoNumber).HasColumnName("po_number").IsRequired().HasMaxLength(50);
        builder.Property(p => p.SupplierId).HasColumnName("supplier_id").IsRequired();
        builder.Property(p => p.OrderDate).HasColumnName("order_date").HasColumnType("date");
        builder.Property(p => p.ExpectedDate).HasColumnName("expected_date").HasColumnType("date");
        builder.Property(p => p.Status).HasColumnName("status").IsRequired().HasMaxLength(30);
        builder.Property(p => p.Notes).HasColumnName("notes");
        builder.Property(p => p.CreatedBy).HasColumnName("created_by").HasMaxLength(50);
        builder.Property(p => p.CreatedDate).HasColumnName("created_date").HasColumnType("datetime2").HasDefaultValueSql("GETDATE()");
        builder.Property(p => p.ModifiedDate).HasColumnName("modified_date").HasColumnType("datetime2").HasDefaultValueSql("GETDATE()");

        builder.HasIndex(p => p.PoNumber).IsUnique();
        builder.HasIndex(p => p.SupplierId).HasDatabaseName("IX_PurchaseOrder_Supplier");

        builder.HasMany(p => p.Lines)
            .WithOne(l => l.PurchaseOrder)
            .HasForeignKey(l => l.PoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(p => p.DomainEvents);
    }
}
