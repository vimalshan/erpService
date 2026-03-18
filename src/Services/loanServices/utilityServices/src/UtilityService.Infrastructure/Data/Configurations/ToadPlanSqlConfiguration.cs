using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UtilityService.Domain.Entities;
using UtilityService.Domain.ValueObjects;

namespace UtilityService.Infrastructure.Data.Configurations;

public class ToadPlanSqlConfiguration : IEntityTypeConfiguration<ToadPlanSql>
{
    public void Configure(EntityTypeBuilder<ToadPlanSql> builder)
    {
        builder.ToTable("TOAD_PLAN_SQL");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("ID")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Username)
            .HasColumnName("USERNAME")
            .HasMaxLength(30)
            .IsUnicode(false);

        builder.Property(e => e.StatementId)
            .HasColumnName("STATEMENT_ID")
            .HasMaxLength(32)
            .IsUnicode(false)
            .HasConversion(
                v => v.Value,
                v => StatementId.Create(v))
            .IsRequired();

        builder.Property(e => e.Timestamp)
            .HasColumnName("TIMESTAMP")
            .HasColumnType("datetime2(3)");

        builder.Property(e => e.Statement)
            .HasColumnName("STATEMENT")
            .HasMaxLength(2000)
            .IsUnicode(false);

        builder.Property(e => e.IsDeleted)
            .HasColumnName("IS_DELETED")
            .HasDefaultValue(false);

        builder.Property(e => e.CreatedAt)
            .HasColumnName("CREATED_AT")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(e => e.UpdatedAt)
            .HasColumnName("UPDATED_AT");

        builder.HasIndex(e => e.StatementId)
            .HasDatabaseName("IX_TOAD_PLAN_SQL_STATEMENT_ID");

        builder.HasIndex(e => e.Username)
            .HasDatabaseName("IX_TOAD_PLAN_SQL_USERNAME");

        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}
