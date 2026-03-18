using AccountingService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountingService.Infrastructure.Data.Configurations;

public class TransactionDetailConfiguration : IEntityTypeConfiguration<TransactionDetail>
{
    public void Configure(EntityTypeBuilder<TransactionDetail> builder)
    {
        builder.ToTable("TRAN_DET");
        builder.HasKey(x => new { x.TdTrustCode, x.TransactionId });
        builder.Property(x => x.TdTrustCode).HasColumnName("TD_TRUST_CODE").HasColumnType("CHAR(3)").IsRequired();
        builder.Property(x => x.TransactionId).HasColumnName("TRANSACTION_ID").IsRequired();
        builder.Property(x => x.TdTransactionCode).HasColumnName("TD_TRANSACTION_CODE").HasColumnType("CHAR(3)").IsRequired();
        builder.Property(x => x.TdTransactionType).HasColumnName("TD_TRANSACTION_TYPE").HasColumnType("CHAR(1)");
        builder.Property(x => x.TdTransactionDate).HasColumnName("TD_TRANSACTION_DATE").HasColumnType("DATETIME2(3)").IsRequired();
        builder.Property(x => x.TdAmount).HasColumnName("TD_AMOUNT").HasColumnType("DECIMAL(19,0)").IsRequired();
        builder.Property(x => x.TdRemarks).HasColumnName("TD_REMARKS").HasMaxLength(255);
        builder.Property(x => x.TdMemberNo).HasColumnName("TD_MEMBER_NO");
        builder.Property(x => x.TdReferenceType).HasColumnName("TD_REFERENCE_TYPE").HasMaxLength(25);
        builder.Property(x => x.TdContributionReferenceNo).HasColumnName("TD_CONTRIBUTION_REFERENCE_NO").HasMaxLength(255);
        builder.Property(x => x.TdTypeCode).HasColumnName("TD_TYPE_CODE").HasColumnType("CHAR(1)").IsRequired();
        builder.Property(x => x.TdLastModifiedOn).HasColumnName("TD_LAST_MODIFIED_ON").HasColumnType("DATETIME2(3)").IsRequired();
        builder.Property(x => x.TdLastModifiedEmpSysid).HasColumnName("TD_LAST_MODIFIED_EMP_SYSID").HasMaxLength(50).IsRequired();
        builder.Property(x => x.TdFinyear).HasColumnName("TD_FINYEAR").IsRequired();
        builder.Property(x => x.TdJvVoucherType).HasColumnName("TD_JV_VOUCHER_TYPE").HasColumnType("CHAR(3)").IsRequired();
        builder.Property(x => x.TdJvNo).HasColumnName("TD_JV_NO").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TdCancelStatus).HasColumnName("TD_CANCEL_STATUS");
        builder.Property(x => x.TdCancelDate).HasColumnName("TD_CANCEL_DATE").HasColumnType("DATETIME2(3)");
        builder.Property(x => x.TdTrnSubType).HasColumnName("TD_TRN_SUB_TYPE").HasColumnType("CHAR(3)");

        builder.HasIndex(x => new { x.TdTransactionType, x.TdTransactionDate }).HasDatabaseName("IDX_TRAN_DET_TYPE");
    }
}
