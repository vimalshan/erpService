using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PayrollServices.Domain.Entities;

namespace PayrollServices.Infrastructure.Data.Configurations;

public class PayrollTransactionConfiguration : IEntityTypeConfiguration<PayrollTransaction>
{
    public void Configure(EntityTypeBuilder<PayrollTransaction> builder)
    {
        builder.ToTable("PAY_TRANDET");

        builder.HasKey(x => x.TransactionId);
        builder.Property(x => x.TransactionId).HasColumnName("TRN_ID").ValueGeneratedOnAdd();
        builder.Property(x => x.EmployeeSystemId).HasColumnName("TRN_EMPSYSID").IsRequired();
        builder.Property(x => x.BatchId).HasColumnName("TRN_BATCHID").IsRequired();
        builder.Property(x => x.Month).HasColumnName("TRN_MONTH").HasMaxLength(7).IsRequired();
        builder.Property(x => x.GrossSalary).HasColumnName("TRN_GROSS").HasPrecision(19, 0);
        builder.Property(x => x.Deductions).HasColumnName("TRN_DEDUCTIONS").HasPrecision(19, 0);
        builder.Property(x => x.NetSalary).HasColumnName("TRN_NET").HasPrecision(19, 0);
        builder.Property(x => x.Status).HasColumnName("TRN_STATUS").HasConversion<string>();
        builder.Property(x => x.CreatedBy).HasColumnName("TRN_CREATEDBY").IsRequired();
        builder.Property(x => x.CreatedOn).HasColumnName("TRN_CREATEDON").IsRequired();
        builder.Property(x => x.UpdatedOn).HasColumnName("TRN_UPDATEDON");
        builder.Property(x => x.UpdatedBy).HasColumnName("TRN_UPDATEDBY");

        builder.HasIndex(x => new { x.EmployeeSystemId, x.Month });
        builder.HasIndex(x => x.BatchId);
    }
}
