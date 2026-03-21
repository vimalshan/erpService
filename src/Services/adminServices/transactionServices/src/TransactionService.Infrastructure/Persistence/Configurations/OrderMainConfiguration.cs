namespace TransactionService.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionService.Domain.Entities;

public sealed class OrderMainConfiguration : IEntityTypeConfiguration<OrderMain>
{
    public void Configure(EntityTypeBuilder<OrderMain> builder)
    {
        builder.ToTable("SP_ORDER_MAIN");
        builder.HasKey(o => o.OrderMainId);
        builder.Property(o => o.OrderMainId).HasColumnName("OM_ORDERMAIN_ID").ValueGeneratedNever();
        builder.Property(o => o.LocationId).HasColumnName("OM_LOCATION_ID");
        builder.Property(o => o.VendorId).HasColumnName("OM_VENDORID");
        builder.Property(o => o.DeliveryDate).HasColumnName("OM_DELIVERYDATE");
        builder.Property(o => o.OrderedDate).HasColumnName("OM_ORDEREDDATE");
        builder.Property(o => o.OrderedBy).HasColumnName("OM_ORDEREDBY");

        builder.HasMany(o => o.Details)
            .WithOne(s => s.OrderMain)
            .HasForeignKey(s => s.OrderMainId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(o => o.Details).UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Ignore(o => o.DomainEvents);
    }
}
