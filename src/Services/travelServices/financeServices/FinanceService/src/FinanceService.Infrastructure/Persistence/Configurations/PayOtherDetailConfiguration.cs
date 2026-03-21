using FinanceService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceService.Infrastructure.Persistence.Configurations;

public class PayOtherDetailConfiguration : IEntityTypeConfiguration<PayOtherDetail>
{
    public void Configure(EntityTypeBuilder<PayOtherDetail> builder)
    {
        builder.ToTable("PAY_OTHDET");
        builder.HasKey(e => e.CompanyCode);
        builder.Property(e => e.CompanyCode).HasColumnName("PY_COM_COD").ValueGeneratedNever();
        builder.Property(e => e.TransactionNumber).HasColumnName("PY_TRN_NUM");
        builder.Property(e => e.PayBatchNo).HasColumnName("PY_PAY_NUM");
        builder.Property(e => e.VendorCode).HasColumnName("PY_VND_COD");
        builder.Property(e => e.TransactionDate).HasColumnName("PY_TRN_DAT");
        builder.Property(e => e.PayMode).HasColumnName("PY_PAY_MOD").HasMaxLength(3).IsFixedLength();
        builder.Property(e => e.PayAmount).HasColumnName("PY_PAY_AMT").HasColumnType("decimal(19,0)");
        builder.Property(e => e.ChequeDate).HasColumnName("PY_CHQ_DAT");
        builder.Property(e => e.ChequeNumber).HasColumnName("PY_CHQ_NUM");
        builder.Property(e => e.PayDate).HasColumnName("PY_PAY_DAT");
        builder.Property(e => e.Remarks).HasColumnName("PY_REM_MRK").HasMaxLength(4000);
        builder.Property(e => e.StatusCode).HasColumnName("PY_STS_COD").HasMaxLength(1).IsFixedLength();
    }
}
