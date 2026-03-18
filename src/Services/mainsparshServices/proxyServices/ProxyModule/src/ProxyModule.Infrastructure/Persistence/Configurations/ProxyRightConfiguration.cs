using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProxyModule.Domain.Entities;

namespace ProxyModule.Infrastructure.Persistence.Configurations;

public class ProxyRightConfiguration : IEntityTypeConfiguration<ProxyRight>
{
    public void Configure(EntityTypeBuilder<ProxyRight> builder)
    {
        builder.ToTable("PROXY_RIGHTS");

        builder.HasKey(e => e.ProxyId);

        builder.Property(e => e.ProxyId)
            .HasColumnName("PROXY_ID")
            .UseIdentityColumn();

        builder.Property(e => e.ProxyUserId)
            .HasColumnName("PROXY_USER_ID")
            .IsRequired();

        builder.Property(e => e.DelegatedUserId)
            .HasColumnName("DELEGATED_USER_ID")
            .IsRequired();

        builder.Property(e => e.ProxyStartDate)
            .HasColumnName("PROXY_START_DATE")
            .HasColumnType("date")
            .IsRequired();

        builder.Property(e => e.ProxyEndDate)
            .HasColumnName("PROXY_END_DATE")
            .HasColumnType("date");

        builder.Property(e => e.ProxyType)
            .HasColumnName("PROXY_TYPE")
            .HasMaxLength(50);

        builder.Property(e => e.ProxyStatus)
            .HasColumnName("PROXY_STATUS")
            .HasColumnType("char(1)")
            .HasDefaultValue("A");

        builder.Property(e => e.Scope)
            .HasColumnName("SCOPE")
            .HasMaxLength(100);

        builder.Property(e => e.Notes)
            .HasColumnName("NOTES")
            .HasColumnType("nvarchar(max)");

        builder.Property(e => e.CreatedBy)
            .HasColumnName("CREATED_BY")
            .IsRequired();

        builder.Property(e => e.CreatedOn)
            .HasColumnName("CREATED_ON")
            .HasColumnType("datetime2(3)")
            .HasDefaultValueSql("GETDATE()");

        builder.Property(e => e.UpdatedBy)
            .HasColumnName("UPDATED_BY");

        builder.Property(e => e.UpdatedOn)
            .HasColumnName("UPDATED_ON")
            .HasColumnType("datetime2(3)");

        // Indexes
        builder.HasIndex(e => e.ProxyUserId).HasDatabaseName("IX_PROXY_RIGHTS_PROXY_USER_ID");
        builder.HasIndex(e => e.DelegatedUserId).HasDatabaseName("IX_PROXY_RIGHTS_DELEGATED_USER_ID");
        builder.HasIndex(e => e.ProxyStatus).HasDatabaseName("IX_PROXY_RIGHTS_STATUS");
        builder.HasIndex(e => new { e.ProxyStartDate, e.ProxyEndDate }).HasDatabaseName("IX_PROXY_RIGHTS_DATES");
        builder.HasIndex(e => e.ProxyType).HasDatabaseName("IX_PROXY_RIGHTS_TYPE");

        builder.Ignore(e => e.DomainEvents);

        // ── Seed Data ──────────────────────────────────────────────────────────
        // Anonymous types are used so EF Core can assign private-setter properties.
        var seedCreatedOn = new DateTime(2026, 3, 15, 0, 0, 0, DateTimeKind.Utc);

        builder.HasData(
            new
            {
                ProxyId = 1L,
                ProxyUserId = 100L,
                DelegatedUserId = 101L,
                ProxyStartDate = new DateTime(2026, 3, 15, 0, 0, 0, DateTimeKind.Utc),
                ProxyEndDate = (DateTime?)new DateTime(2026, 4, 14, 0, 0, 0, DateTimeKind.Utc),
                ProxyType = "APPROVAL",
                ProxyStatus = "A",
                Scope = (string?)"DEPARTMENT",
                Notes = (string?)"Approval delegation for Q1 reviews",
                CreatedBy = 1L,
                CreatedOn = seedCreatedOn,
                UpdatedBy = (long?)null,
                UpdatedOn = (DateTime?)null
            },
            new
            {
                ProxyId = 2L,
                ProxyUserId = 200L,
                DelegatedUserId = 201L,
                ProxyStartDate = new DateTime(2026, 3, 15, 0, 0, 0, DateTimeKind.Utc),
                ProxyEndDate = (DateTime?)new DateTime(2026, 5, 14, 0, 0, 0, DateTimeKind.Utc),
                ProxyType = "SUBMISSION",
                ProxyStatus = "A",
                Scope = (string?)"ALL",
                Notes = (string?)"Submission delegation during leave",
                CreatedBy = 1L,
                CreatedOn = seedCreatedOn,
                UpdatedBy = (long?)null,
                UpdatedOn = (DateTime?)null
            },
            new
            {
                ProxyId = 3L,
                ProxyUserId = 300L,
                DelegatedUserId = 301L,
                ProxyStartDate = new DateTime(2026, 3, 15, 0, 0, 0, DateTimeKind.Utc),
                ProxyEndDate = (DateTime?)null,
                ProxyType = "FULL",
                ProxyStatus = "A",
                Scope = (string?)"LOCATION",
                Notes = (string?)"Permanent full proxy for branch office",
                CreatedBy = 1L,
                CreatedOn = seedCreatedOn,
                UpdatedBy = (long?)null,
                UpdatedOn = (DateTime?)null
            },
            new
            {
                ProxyId = 4L,
                ProxyUserId = 400L,
                DelegatedUserId = 401L,
                ProxyStartDate = new DateTime(2026, 3, 15, 0, 0, 0, DateTimeKind.Utc),
                ProxyEndDate = (DateTime?)new DateTime(2026, 3, 22, 0, 0, 0, DateTimeKind.Utc),
                ProxyType = "READONLY",
                ProxyStatus = "A",
                Scope = (string?)"SPECIFIC",
                Notes = (string?)"Temporary read-only access for audit",
                CreatedBy = 1L,
                CreatedOn = seedCreatedOn,
                UpdatedBy = (long?)null,
                UpdatedOn = (DateTime?)null
            },
            new
            {
                ProxyId = 5L,
                ProxyUserId = 500L,
                DelegatedUserId = 501L,
                ProxyStartDate = new DateTime(2026, 3, 15, 0, 0, 0, DateTimeKind.Utc),
                ProxyEndDate = (DateTime?)new DateTime(2026, 6, 13, 0, 0, 0, DateTimeKind.Utc),
                ProxyType = "APPROVAL",
                ProxyStatus = "A",
                Scope = (string?)"ALL",
                Notes = (string?)"Long-term approval proxy for annual cycle",
                CreatedBy = 1L,
                CreatedOn = seedCreatedOn,
                UpdatedBy = (long?)null,
                UpdatedOn = (DateTime?)null
            }
        );
    }
}
