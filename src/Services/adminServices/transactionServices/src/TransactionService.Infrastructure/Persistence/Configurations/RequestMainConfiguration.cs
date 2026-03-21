namespace TransactionService.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionService.Domain.Entities;
using TransactionService.Domain.ValueObjects;

public sealed class RequestMainConfiguration : IEntityTypeConfiguration<RequestMain>
{
    public void Configure(EntityTypeBuilder<RequestMain> builder)
    {
        builder.ToTable("SP_REQUEST_MAIN");
        builder.HasKey(r => r.RequestId);
        builder.Property(r => r.RequestId).HasColumnName("RM_REQUESTID").ValueGeneratedNever();
        builder.Property(r => r.RequestedBy).HasColumnName("RM_REQUESTEDBY");
        builder.Property(r => r.RequestedOn).HasColumnName("RM_REQUESTEDON");
        builder.Property(r => r.LocationId).HasColumnName("RM_LOCATIONID");
        builder.Property(r => r.UnitCode)
            .HasColumnName("RM_UNITCODE")
            .HasMaxLength(3)
            .HasConversion(
                u => u == null ? null : u.Value,
                v => v == null ? null : new UnitCode(v));

        builder.HasMany(r => r.Details)
            .WithOne(s => s.RequestMain)
            .HasForeignKey(s => s.RequestId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(r => r.Details).UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Ignore(r => r.DomainEvents);
    }
}
