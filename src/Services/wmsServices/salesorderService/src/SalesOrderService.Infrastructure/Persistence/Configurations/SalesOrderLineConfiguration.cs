using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesOrderService.Domain.Entities;

namespace SalesOrderService.Infrastructure.Persistence.Configurations;

public sealed class SalesOrderLineConfiguration : IEntityTypeConfiguration<SalesOrderLine>
{
    public void Configure(EntityTypeBuilder<SalesOrderLine> builder)
    {
        builder.ToTable("SalesOrderLine");

        builder.HasKey(x => x.SoLineId);
        builder.Property(x => x.SoLineId)
               .HasColumnName("so_line_id")
               .UseIdentityColumn();

        builder.Property(x => x.SoId)
               .HasColumnName("so_id")
               .IsRequired();

        builder.HasIndex(x => x.SoId)
               .HasDatabaseName("IX_SalesOrderLine_SO");

        builder.Property(x => x.ProductId)
               .HasColumnName("product_id")
               .IsRequired();

        builder.HasIndex(x => x.ProductId)
               .HasDatabaseName("IX_SalesOrderLine_Product");

        builder.Property(x => x.LineNumber)
               .HasColumnName("line_number")
               .IsRequired();

        builder.HasIndex(x => new { x.SoId, x.LineNumber })
               .IsUnique()
               .HasDatabaseName("UQ_SOLine_SO_Line");

        builder.Property(x => x.QuantityOrdered)
               .HasColumnName("quantity_ordered")
               .HasColumnType("decimal(18,3)")
               .IsRequired();

        builder.Property(x => x.QuantityShipped)
               .HasColumnName("quantity_shipped")
               .HasColumnType("decimal(18,3)")
               .IsRequired()
               .HasDefaultValue(0m);

        builder.Property(x => x.UnitPrice)
               .HasColumnName("unit_price")
               .HasColumnType("decimal(18,4)");

        builder.Property(x => x.Discount)
               .HasColumnName("discount")
               .HasColumnType("decimal(18,2)")
               .HasDefaultValue(0m);

        builder.Property(x => x.Notes)
               .HasColumnName("notes");

        builder.Ignore(x => x.LineTotal);
        builder.Ignore(x => x.DomainEvents);
    }
}
