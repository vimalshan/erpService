using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CurrencyManagement.Domain.Entities;

namespace CurrencyManagement.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for Currency entity
/// </summary>
public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
{
    public void Configure(EntityTypeBuilder<Currency> builder)
    {
        builder.ToTable("DEAL_CURRMAST");

        builder.HasKey(c => c.CurrencyId)
            .HasName("PK_DEAL_CURRMAST");

        builder.Property(c => c.CurrencyId)
            .HasColumnName("CURR_ID")
            .ValueGeneratedNever();

        builder.Property(c => c.Name)
            .HasColumnName("CURR_NAME")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(c => c.Symbol)
            .HasColumnName("CURR_SYMBOL")
            .HasMaxLength(25)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => CurrencyManagement.Domain.ValueObjects.CurrencySymbol.Create(v));

        builder.Property(c => c.ModifiedBy)
            .HasColumnName("CURR_MODIFIEDBY")
            .IsRequired();

        builder.Property(c => c.ModifiedOn)
            .HasColumnName("CURR_MODIFIEDON")
            .HasColumnType("datetime2(3)")
            .IsRequired();

        builder.Ignore(c => c.DomainEvents);
    }
}

/// <summary>
/// EF Core configuration for ExchangeRate entity
/// </summary>
public class ExchangeRateConfiguration : IEntityTypeConfiguration<ExchangeRate>
{
    public void Configure(EntityTypeBuilder<ExchangeRate> builder)
    {
        builder.ToTable("DEAL_CURRATES");

        builder.HasKey(e => e.RateId)
            .HasName("PK_DEAL_CURRATES");

        builder.Property(e => e.RateId)
            .HasColumnName("CURRATE_ID")
            .ValueGeneratedNever();

        builder.Property(e => e.FinancialYear)
            .HasColumnName("CURRATE_FINYEAR")
            .IsRequired();

        builder.Property(e => e.Month)
            .HasColumnName("CURRATE_MONTH")
            .IsRequired();

        builder.Property(e => e.FromCurrencyId)
            .HasColumnName("CURRATE_FROMCUR")
            .IsRequired();

        builder.Property(e => e.ToCurrencyId)
            .HasColumnName("CURRATE_TOCUR")
            .IsRequired();

        builder.Property(e => e.Rate)
            .HasColumnName("CURRATE_RATE")
            .HasColumnType("DECIMAL(19,6)")
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => CurrencyManagement.Domain.ValueObjects.ExchangeRateValue.Create(v));

        builder.Property(e => e.ModifiedBy)
            .HasColumnName("CURRATE_MODIFIEDBY")
            .IsRequired();

        builder.Property(e => e.ModifiedOn)
            .HasColumnName("CURRATE_MODIFIEDON")
            .HasColumnType("datetime2(3)")
            .IsRequired();

        // Indexes
        builder.HasIndex(e => new { e.FinancialYear, e.Month })
            .HasDatabaseName("IX_DEAL_CURRATES_FINYEAR_MONTH");

        builder.HasIndex(e => new { e.FromCurrencyId, e.ToCurrencyId })
            .HasDatabaseName("IX_DEAL_CURRATES_FROMCUR_TOCUR");

        builder.Ignore(e => e.DomainEvents);
    }
}

/// <summary>
/// EF Core configuration for OrganizationCurrencyMapping entity
/// </summary>
public class OrganizationCurrencyMappingConfiguration : IEntityTypeConfiguration<OrganizationCurrencyMapping>
{
    public void Configure(EntityTypeBuilder<OrganizationCurrencyMapping> builder)
    {
        builder.ToTable("DEAL_ORGCURRMAP");

        builder.HasKey(o => new { o.OrganizationId, o.CurrencyId })
            .HasName("PK_DEAL_ORGCURRMAP");

        builder.Property(o => o.OrganizationId)
            .HasColumnName("ORG_ID")
            .IsRequired();

        builder.Property(o => o.CurrencyId)
            .HasColumnName("ORG_CURRID")
            .IsRequired();

        builder.Property(o => o.ModifiedBy)
            .HasColumnName("ORG_MODIFIEDBY")
            .IsRequired();

        builder.Property(o => o.ModifiedOn)
            .HasColumnName("ORG_MODIFIEDON")
            .HasColumnType("datetime2(3)")
            .IsRequired();

        // Foreign key
        builder.HasOne<Currency>()
            .WithMany()
            .HasForeignKey(o => o.CurrencyId)
            .HasConstraintName("FK_DEAL_ORGCURRMAP_CURRMAST");

        // Index
        builder.HasIndex(o => o.OrganizationId)
            .HasDatabaseName("IX_DEAL_ORGCURRMAP_ORG_ID");

        builder.Ignore(o => o.DomainEvents);
    }
}
