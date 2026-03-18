using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PayrollServices.Domain.Entities;

namespace PayrollServices.Infrastructure.Data.Configurations;

public class PayrollAdjustmentConfiguration : IEntityTypeConfiguration<PayrollAdjustment>
{
    public void Configure(EntityTypeBuilder<PayrollAdjustment> builder)
    {
        builder.ToTable("PAY_ARR");

        builder.HasKey(x => x.AdjustmentId);
        builder.Property(x => x.AdjustmentId).HasColumnName("AR_ID").ValueGeneratedNever();
        builder.Property(x => x.EmployeeSystemId).HasColumnName("PAY_EMPSYSID").IsRequired();
        builder.Property(x => x.Amount).HasColumnName("AR_AMOUNT").HasPrecision(19, 0);
        builder.Property(x => x.AdjustmentType).HasColumnName("AR_TYPE").HasConversion<string>();
        builder.Property(x => x.AdjustmentDate).HasColumnName("AR_DATE").IsRequired();
        builder.Property(x => x.Description).HasColumnName("AR_DESCRIPTION").HasMaxLength(500);
        builder.Property(x => x.CreatedBy).HasColumnName("AR_CREATEDBY").IsRequired();
        builder.Property(x => x.CreatedOn).HasColumnName("AR_CREATEDON").IsRequired();
        builder.Property(x => x.ApprovedOn).HasColumnName("AR_APPROVEDON");
        builder.Property(x => x.ApprovedBy).HasColumnName("AR_APPROVEDBY");

        builder.HasIndex(x => x.EmployeeSystemId);
        builder.HasIndex(x => new { x.EmployeeSystemId, x.AdjustmentDate });
    }
}
