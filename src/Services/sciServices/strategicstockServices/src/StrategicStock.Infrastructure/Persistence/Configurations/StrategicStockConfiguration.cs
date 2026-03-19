using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StrategicStock.Domain.Entities;
using StrategicStock.Domain.ValueObjects;

namespace StrategicStock.Infrastructure.Persistence.Configurations;

public sealed class StrategicStockConfiguration : IEntityTypeConfiguration<StrategicStockEntity>
{
    public void Configure(EntityTypeBuilder<StrategicStockEntity> builder)
    {
        builder.ToTable("STRATEGIC_STOCK");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("STRATEGIC_STOCK_ID")
            .ValueGeneratedNever();

        builder.Property(e => e.CompanyUnitId)
            .HasColumnName("COMPANY_UNIT_ID");

        builder.Property(e => e.SciItemId)
            .HasColumnName("SCI_ITEM_ID")
            .IsRequired();

        builder.Property(e => e.StockType)
            .HasColumnName("STRATEGIC_STOCK_TYPE")
            .HasMaxLength(2)
            .HasConversion(
                v => v != null ? v.Code : null,
                v => v != null ? StrategicStockType.FromCode(v) : null);

        builder.Property(e => e.MaxQty)
            .HasColumnName("MAX_QTY")
            .HasConversion(
                v => v != null ? (long?)v.Value : null,
                v => v.HasValue ? StockQuantity.Create(v.Value) : null);

        builder.Property(e => e.EffectiveDate)
            .HasColumnName("EFFECTIVE_DATE")
            .HasMaxLength(255);

        builder.Property(e => e.ClosureDate)
            .HasColumnName("CLOSURE_DATE")
            .HasMaxLength(255);

        builder.Property(e => e.SciUserIdCreated)
            .HasColumnName("SCI_USER_ID_CREATED");

        builder.Property(e => e.CreationDate)
            .HasColumnName("CREATION_DATE")
            .HasPrecision(3)
            .IsRequired();

        builder.Property(e => e.SciUserIdModified)
            .HasColumnName("SCI_USER_ID_MODIFIED");

        builder.Property(e => e.ModifiedDate)
            .HasColumnName("MODIFIED_DATE")
            .HasMaxLength(255);

        builder.Property(e => e.FilledQty)
            .HasColumnName("FILLED_QTY")
            .HasConversion(
                v => v != null ? (long?)v.Value : null,
                v => v.HasValue ? StockQuantity.Create(v.Value) : null);

        builder.Ignore(e => e.DomainEvents);

        // Seed data
        builder.HasData(
            new
            {
                Id = 1,
                CompanyUnitId = (int?)1,
                SciItemId = 1001,
                StockType = StrategicStockType.Normal,
                MaxQty = StockQuantity.Create(5000),
                EffectiveDate = "2026-01-01",
                SciUserIdCreated = (int?)1,
                CreationDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                FilledQty = StockQuantity.Create(1200),
                ModifiedDate = (string?)null,
                ClosureDate = (string?)null,
                SciUserIdModified = (int?)null
            },
            new
            {
                Id = 2,
                CompanyUnitId = (int?)1,
                SciItemId = 1002,
                StockType = StrategicStockType.Emergency,
                MaxQty = StockQuantity.Create(3000),
                EffectiveDate = "2026-02-01",
                SciUserIdCreated = (int?)1,
                CreationDate = new DateTime(2026, 2, 1, 0, 0, 0, DateTimeKind.Utc),
                FilledQty = StockQuantity.Create(800),
                ModifiedDate = (string?)null,
                ClosureDate = (string?)null,
                SciUserIdModified = (int?)null
            },
            new
            {
                Id = 3,
                CompanyUnitId = (int?)2,
                SciItemId = 1003,
                StockType = StrategicStockType.Buffer,
                MaxQty = StockQuantity.Create(10000),
                EffectiveDate = "2026-03-01",
                SciUserIdCreated = (int?)2,
                CreationDate = new DateTime(2026, 3, 1, 0, 0, 0, DateTimeKind.Utc),
                FilledQty = StockQuantity.Create(4500),
                ModifiedDate = (string?)null,
                ClosureDate = (string?)null,
                SciUserIdModified = (int?)null
            },
            new
            {
                Id = 4,
                CompanyUnitId = (int?)2,
                SciItemId = 1004,
                StockType = StrategicStockType.Normal,
                MaxQty = StockQuantity.Create(2000),
                EffectiveDate = "2025-06-15",
                SciUserIdCreated = (int?)1,
                CreationDate = new DateTime(2025, 6, 15, 0, 0, 0, DateTimeKind.Utc),
                FilledQty = StockQuantity.Create(2000),
                ModifiedDate = "2025-12-31",
                ClosureDate = "2025-12-31",
                SciUserIdModified = (int?)1
            },
            new
            {
                Id = 5,
                CompanyUnitId = (int?)1,
                SciItemId = 1005,
                StockType = StrategicStockType.Emergency,
                MaxQty = StockQuantity.Create(7500),
                EffectiveDate = "2026-01-15",
                SciUserIdCreated = (int?)2,
                CreationDate = new DateTime(2026, 1, 15, 0, 0, 0, DateTimeKind.Utc),
                FilledQty = StockQuantity.Zero,
                ModifiedDate = (string?)null,
                ClosureDate = (string?)null,
                SciUserIdModified = (int?)null
            });
    }
}
