using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionService.Domain.Entities;

namespace TransactionService.Infrastructure.Persistence.Configurations;

public sealed class EmployeePaymentConfiguration : IEntityTypeConfiguration<EmployeePayment>
{
    public void Configure(EntityTypeBuilder<EmployeePayment> builder)
    {
        builder.ToTable("JVEMPPAY_DET");
        builder.HasKey(x => x.PayId);

        builder.Property(x => x.PayId).HasColumnName("PAY_ID").ValueGeneratedNever();
        builder.Property(x => x.PayTpId).HasColumnName("PAY_TPID").IsRequired();
        builder.Property(x => x.PayTrnType).HasColumnName("PAY_TRNTYPE").HasMaxLength(3).IsRequired();
        builder.Property(x => x.PayEmpSysId).HasColumnName("PAY_EMPSYSID").IsRequired();
        builder.Property(x => x.PayUnitId).HasColumnName("PAY_UNITID").IsRequired();
        builder.Property(x => x.PayMode).HasColumnName("PAY_MODE").HasMaxLength(3).IsRequired();
        builder.Property(x => x.PayType).HasColumnName("PAY_TYPE").HasMaxLength(3).IsRequired();
        builder.Property(x => x.PayDate).HasColumnName("PAY_DATE");
        builder.Property(x => x.PayAmount).HasColumnName("PAY_AMOUNT").HasColumnType("DECIMAL(19,0)").IsRequired();
        builder.Property(x => x.PayRefId).HasColumnName("PAY_REFID").IsRequired();
        builder.Property(x => x.PayBatchId).HasColumnName("PAY_BATCHID").IsRequired();
        builder.Property(x => x.PayJvId).HasColumnName("PAY_JVID").IsRequired();
        builder.Property(x => x.CreatedBy).HasColumnName("PAY_CREATEDBY").IsRequired();
        builder.Property(x => x.CreatedOn).HasColumnName("PAY_CREATEDON").IsRequired();

        builder.Ignore(x => x.ModifiedBy);
        builder.Ignore(x => x.ModifiedOn);
        builder.Ignore(x => x.DomainEvents);
    }
}
