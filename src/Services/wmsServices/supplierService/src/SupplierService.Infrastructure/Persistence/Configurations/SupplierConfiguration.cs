using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SupplierService.Domain.Entities;

namespace SupplierService.Infrastructure.Persistence.Configurations;

public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        builder.ToTable("Supplier");

        builder.HasKey(s => s.SupplierId);
        builder.Property(s => s.SupplierId).HasColumnName("supplier_id");

        builder.Property(s => s.Code)
            .HasColumnName("code")
            .HasMaxLength(30)
            .IsRequired();

        builder.HasIndex(s => s.Code).IsUnique();

        builder.Property(s => s.Name)
            .HasColumnName("name")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(s => s.ContactPerson)
            .HasColumnName("contact_person")
            .HasMaxLength(100);

        builder.Property(s => s.Email)
            .HasColumnName("email")
            .HasMaxLength(100);

        builder.Property(s => s.Phone)
            .HasColumnName("phone")
            .HasMaxLength(30);

        builder.Property(s => s.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true);

        builder.Property(s => s.CreatedDate)
            .HasColumnName("created_date")
            .HasDefaultValueSql("GETDATE()");

        builder.Property(s => s.ModifiedDate)
            .HasColumnName("modified_date")
            .HasDefaultValueSql("GETDATE()");

        builder.OwnsOne(s => s.Address, a =>
        {
            a.Property(p => p.Street).HasColumnName("address").HasMaxLength(200);
            a.Property(p => p.City).HasColumnName("city").HasMaxLength(50);
            a.Property(p => p.State).HasColumnName("state").HasMaxLength(50);
            a.Property(p => p.Country).HasColumnName("country").HasMaxLength(50);
            a.Property(p => p.PostalCode).HasColumnName("postal_code").HasMaxLength(20);
        });

        builder.Ignore(s => s.DomainEvents);
    }
}
