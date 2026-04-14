using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMTransactional.Domain.Entities;

namespace WMTransactional.Infrastructure.Persistence.Configurations;

public class ReceivingConfiguration : IEntityTypeConfiguration<Receiving>
{
    public void Configure(EntityTypeBuilder<Receiving> builder)
    {
        builder.ToTable("Receiving");
        builder.HasKey(r => r.ReceivingId);
        builder.Property(r => r.ReceivingId).HasColumnName("receiving_id").ValueGeneratedOnAdd();
        builder.Property(r => r.ReceivingNumber).HasColumnName("receiving_number").IsRequired().HasMaxLength(50);
        builder.Property(r => r.PoId).HasColumnName("po_id").IsRequired();
        builder.Property(r => r.ReceivedDate).HasColumnName("received_date").HasColumnType("datetime2").HasDefaultValueSql("GETDATE()");
        builder.Property(r => r.Status).HasColumnName("status").IsRequired().HasMaxLength(30);
        builder.Property(r => r.Notes).HasColumnName("notes");
        builder.Property(r => r.CreatedBy).HasColumnName("created_by").HasMaxLength(50);
        builder.Property(r => r.CreatedDate).HasColumnName("created_date").HasColumnType("datetime2").HasDefaultValueSql("GETDATE()");

        builder.HasIndex(r => r.ReceivingNumber).IsUnique();
        builder.HasIndex(r => r.PoId).HasDatabaseName("IX_Receiving_PO");

        builder.HasOne(r => r.PurchaseOrder).WithMany().HasForeignKey(r => r.PoId).OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(r => r.Lines)
            .WithOne(l => l.Receiving)
            .HasForeignKey(l => l.ReceivingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(r => r.DomainEvents);
    }
}
