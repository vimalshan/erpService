using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PayrollServices.Domain.Entities;

namespace PayrollServices.Infrastructure.Data.Configurations;

public class PayrollBatchConfiguration : IEntityTypeConfiguration<PayrollBatch>
{
    public void Configure(EntityTypeBuilder<PayrollBatch> builder)
    {
        builder.ToTable("PAYROLL_BATCH");

        builder.HasKey(x => x.BatchId);
        builder.Property(x => x.BatchId).HasColumnName("BATCH_ID").ValueGeneratedNever();
        builder.Property(x => x.BatchMonth).HasColumnName("BATCH_MONTH").HasMaxLength(7).IsRequired();
        builder.Property(x => x.Status).HasColumnName("BATCH_STATUS").HasConversion<string>();
        builder.Property(x => x.CreatedBy).HasColumnName("BATCH_CREATEDBY").IsRequired();
        builder.Property(x => x.CreatedOn).HasColumnName("BATCH_CREATEDON").IsRequired();
        builder.Property(x => x.UpdatedOn).HasColumnName("BATCH_UPDATEDON");
        builder.Property(x => x.UpdatedBy).HasColumnName("BATCH_UPDATEDBY");

        builder
            .HasMany(x => x.Transactions)
            .WithOne(x => x.Batch)
            .HasForeignKey(x => x.BatchId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.BatchMonth).IsUnique();
    }
}
