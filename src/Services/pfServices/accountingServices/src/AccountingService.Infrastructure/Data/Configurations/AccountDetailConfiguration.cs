using AccountingService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountingService.Infrastructure.Data.Configurations;

public class AccountDetailConfiguration : IEntityTypeConfiguration<AccountDetail>
{
    public void Configure(EntityTypeBuilder<AccountDetail> builder)
    {
        builder.ToTable("ACC_DET");
        builder.HasKey(x => x.AcSysId);
        builder.Property(x => x.AcSysId).HasColumnName("AC_SYS_ID").ValueGeneratedNever();
        builder.Property(x => x.AcTrustCode).HasColumnName("AC_TRUST_CODE").HasColumnType("CHAR(3)").IsRequired();
        builder.Property(x => x.AcTranCode).HasColumnName("AC_TRAN_CODE").HasColumnType("CHAR(3)").IsRequired();
        builder.Property(x => x.AcTranNo).HasColumnName("AC_TRAN_NO").IsRequired();
        builder.Property(x => x.AcDocNo).HasColumnName("AC_DOC_NO").IsRequired();
        builder.Property(x => x.AcFinYer).HasColumnName("AC_FIN_YER").IsRequired();
        builder.Property(x => x.AcDocDat).HasColumnName("AC_DOC_DAT").HasColumnType("DATETIME2(3)").IsRequired();
        builder.Property(x => x.AcMainCode).HasColumnName("AC_MAIN_CODE").HasColumnType("CHAR(6)").IsRequired();
        builder.Property(x => x.AcSubCode).HasColumnName("AC_SUB_CODE").HasColumnType("CHAR(6)").IsRequired();
        builder.Property(x => x.AcDcType).HasColumnName("AC_DC_TYPE").HasColumnType("CHAR(1)").IsRequired();
        builder.Property(x => x.AcTranAmt).HasColumnName("AC_TRAN_AMT").HasColumnType("DECIMAL(19,0)").IsRequired();
        builder.Property(x => x.AcRefTranCode).HasColumnName("AC_REF_TRANCODE").HasColumnType("CHAR(3)").IsRequired();
        builder.Property(x => x.AcRefTranNo).HasColumnName("AC_REF_TRANNO").IsRequired();
        builder.Property(x => x.AcRemarks).HasColumnName("AC_REMARKS").HasMaxLength(2000);

        builder.HasIndex(x => new { x.AcTrustCode, x.AcDocDat }).HasDatabaseName("IDX_ACC_DET_TRUST_DATE");
    }
}
