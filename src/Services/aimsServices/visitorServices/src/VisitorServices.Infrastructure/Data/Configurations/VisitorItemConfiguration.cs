using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VisitorServices.Domain.Entities;

namespace VisitorServices.Infrastructure.Data.Configurations;

public class VisitorItemConfiguration : IEntityTypeConfiguration<VisitorItem>
{
    public void Configure(EntityTypeBuilder<VisitorItem> builder)
    {
        builder.ToTable("VISITOR_ITEM");

        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id).HasColumnName("ITEM_ID").ValueGeneratedNever();

        builder.Property(i => i.VisitorId).HasColumnName("ITEM_VISITORID").IsRequired();

        builder.Property(i => i.Description)
            .HasColumnName("ITEM_DESCRIPTION")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(i => i.Quantity).HasColumnName("ITEM_QUANTITY").IsRequired();

        builder.Property(i => i.MaterialType)
            .HasColumnName("ITEM_MATERIALTYPE")
            .HasMaxLength(100);

        builder.Property(i => i.Notes)
            .HasColumnName("ITEM_NOTES")
            .HasMaxLength(500);

        builder.Property(i => i.Status)
            .HasColumnName("ITEM_STATUS")
            .HasMaxLength(1)
            .IsRequired()
            .HasConversion(c => c.ToString(), s => s[0]);

        builder.Property(i => i.EnteredOn)
            .HasColumnName("ITEM_ENTEREDON")
            .HasColumnType("datetime2(3)")
            .IsRequired();

        builder.Property(i => i.EnteredBy).HasColumnName("ITEM_ENTEREDBY").IsRequired();

        builder.Ignore(i => i.DomainEvents);

        builder.HasIndex(i => i.VisitorId).HasDatabaseName("IX_VISITOR_ITEM_VISITORID");
    }
}
