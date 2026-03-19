using EmployeePrideManagement.Domain.Entities;
using EmployeePrideManagement.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeePrideManagement.Infrastructure.Data.Configurations;

public class MomentPrideConfiguration : IEntityTypeConfiguration<MomentPride>
{
    public void Configure(EntityTypeBuilder<MomentPride> builder)
    {
        builder.ToTable("MOMENT_PRIDE");

        builder.HasKey(e => e.MomentPrideId);

        builder.Property(e => e.MomentPrideId)
            .HasColumnName("MOMENTPRIDE_ID")
            .HasColumnType("decimal(38,0)")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Title)
            .HasColumnName("MOMENTPRIDE_TITLE")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(e => e.Body)
            .HasColumnName("MOMENTPRIDE_BODY");

        builder.Property(e => e.EmployeeSysId)
            .HasColumnName("MOMENTPRIDE_EMPSYSID")
            .HasColumnType("decimal(38,0)")
            .IsRequired();

        builder.Property(e => e.Footer)
            .HasColumnName("MOMENTPRIDE_FOOTER")
            .HasMaxLength(500)
            .IsRequired();

        builder.OwnsOne(e => e.Location, loc =>
        {
            loc.Property(l => l.Value)
                .HasColumnName("MOMENTPRIDE_LOCATION")
                .HasMaxLength(100)
                .IsRequired();
        });

        builder.OwnsOne(e => e.Image, img =>
        {
            img.Property(i => i.Value)
                .HasColumnName("MOMENTPRIDE_IMAGE")
                .HasMaxLength(200)
                .IsRequired();
        });

        builder.Property(e => e.ModifiedBy)
            .HasColumnName("MOMENTPRIDE_MODIFIEDBY")
            .IsRequired();

        builder.Property(e => e.ModifiedOn)
            .HasColumnName("MOMENTPRIDE_MODIFIEDON")
            .HasColumnType("datetime2(3)");

        builder.HasIndex(e => e.EmployeeSysId)
            .HasDatabaseName("IX_MOMENT_PRIDE_EMPSYSID");

        builder.HasIndex(e => e.ModifiedOn)
            .HasDatabaseName("IX_MOMENT_PRIDE_MODIFIEDON");
    }
}
