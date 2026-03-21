using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReceivingService.Domain.Entities;

namespace ReceivingService.Infrastructure.Data.Configurations;

public sealed class ReceivingConfiguration
    : IEntityTypeConfiguration<Domain.Entities.Receiving>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Receiving> builder)
    {
        builder.ToTable("Receiving");
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
               .HasColumnName("receiving_id")
               .UseIdentityColumn();

        builder.Property(r => r.ReceivingNumber)
               .HasColumnName("receiving_number")
               .IsRequired()
               .HasMaxLength(50);

        builder.HasIndex(r => r.ReceivingNumber).IsUnique()
               .HasDatabaseName("IX_Receiving_ReceivingNumber");

        builder.Property(r => r.PoId)
               .HasColumnName("po_id")
               .IsRequired();

        builder.HasIndex(r => r.PoId)
               .HasDatabaseName("IX_Receiving_PO");

        builder.Property(r => r.WarehouseId)
               .HasColumnName("warehouse_id")
               .IsRequired();

        builder.Property(r => r.ReceivedDate)
               .HasColumnName("received_date")
               .IsRequired();

        builder.Property(r => r.Status)
               .HasColumnName("status")
               .IsRequired()
               .HasMaxLength(30);

        builder.Property(r => r.Notes)
               .HasColumnName("notes");

        builder.Property(r => r.CreatedBy)
               .HasColumnName("created_by")
               .HasMaxLength(50);

        builder.Property(r => r.CreatedDate)
               .HasColumnName("created_date")
               .IsRequired();

        builder.HasMany(r => r.Lines)
               .WithOne(l => l.Receiving)
               .HasForeignKey(l => l.ReceivingId)
               .OnDelete(DeleteBehavior.Cascade);

        // Domain events are not persisted
        builder.Ignore(r => r.DomainEvents);
    }
}
