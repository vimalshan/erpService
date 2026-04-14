using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMTransactional.Domain.Entities;

namespace WMTransactional.Infrastructure.Persistence.Configurations;

public class SalesOrderConfiguration : IEntityTypeConfiguration<SalesOrder>
{
    public void Configure(EntityTypeBuilder<SalesOrder> builder)
    {
        builder.ToTable("SalesOrder");
        builder.HasKey(s => s.SoId);
        builder.Property(s => s.SoId).HasColumnName("so_id").ValueGeneratedOnAdd();
        builder.Property(s => s.SoNumber).HasColumnName("so_number").IsRequired().HasMaxLength(50);
        builder.Property(s => s.CustomerId).HasColumnName("customer_id").IsRequired();
        builder.Property(s => s.OrderDate).HasColumnName("order_date").HasColumnType("date");
        builder.Property(s => s.RequestedDate).HasColumnName("requested_date").HasColumnType("date");
        builder.Property(s => s.Status).HasColumnName("status").IsRequired().HasMaxLength(30);
        builder.Property(s => s.Notes).HasColumnName("notes");
        builder.Property(s => s.CreatedBy).HasColumnName("created_by").HasMaxLength(50);
        builder.Property(s => s.CreatedDate).HasColumnName("created_date").HasColumnType("datetime2").HasDefaultValueSql("GETDATE()");
        builder.Property(s => s.ModifiedDate).HasColumnName("modified_date").HasColumnType("datetime2").HasDefaultValueSql("GETDATE()");

        builder.HasIndex(s => s.SoNumber).IsUnique();
        builder.HasIndex(s => s.CustomerId).HasDatabaseName("IX_SalesOrder_Customer");

        builder.HasMany(s => s.Lines)
            .WithOne(l => l.SalesOrder)
            .HasForeignKey(l => l.SoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(s => s.DomainEvents);
    }
}
