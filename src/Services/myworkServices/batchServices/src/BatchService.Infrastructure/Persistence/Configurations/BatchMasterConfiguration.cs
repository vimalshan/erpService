using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BatchService.Domain.Entities;

namespace BatchService.Infrastructure.Persistence.Configurations;

public sealed class BatchMasterConfiguration : IEntityTypeConfiguration<BatchMaster>
{
    public void Configure(EntityTypeBuilder<BatchMaster> builder)
    {
        builder.ToTable("BATCH_MASTER");

        builder.HasKey(b => b.BatchId);
        builder.Property(b => b.BatchId)
               .HasColumnName("BATCH_ID")
               .ValueGeneratedNever();

        builder.Property(b => b.BatchMonthNo)
               .HasColumnName("BATCH_MONTHNO")
               .IsRequired();

        builder.Property(b => b.BatchStatusChar)
               .HasColumnName("BATCH_STATUS")
               .HasColumnType("char(1)")
               .IsRequired();

        builder.Property(b => b.BatchLastModifiedBy)
               .HasColumnName("BATCH_LASTMODIFIEDBY")
               .IsRequired();

        builder.Property(b => b.BatchLastModifiedOn)
               .HasColumnName("BATCH_LASTMODIFIEDON")
               .HasColumnType("datetime2(3)")
               .IsRequired();

        // Ignore computed value-object properties
        builder.Ignore(b => b.MonthNumber);
        builder.Ignore(b => b.Status);
        builder.Ignore(b => b.DomainEvents);
    }
}
