using DeductionService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeductionService.Infrastructure.Persistence.Configurations;

public class AdhocPayDeductionConfiguration : IEntityTypeConfiguration<AdhocPayDeduction>
{
    public void Configure(EntityTypeBuilder<AdhocPayDeduction> builder)
    {
        builder.ToTable("ADHOC_PAY_DED");
        builder.HasNoKey();

        builder.Property(x => x.SystemId).HasColumnName("PY_SYS_ID");
        builder.Property(x => x.CanteenUnit).HasColumnName("PY_CAN_UNT");
        builder.Property(x => x.SerialNumber).HasColumnName("PY_SRL_NUM");
        builder.Property(x => x.BatchNumber).HasColumnName("PY_BAT_NUM");
        builder.Property(x => x.TransactionDate).HasColumnName("PY_TRN_DAT").HasPrecision(3);
        builder.Property(x => x.EarningDeductionCode).HasColumnName("PY_ED_COD").HasColumnType("CHAR(6)");
        builder.Property(x => x.ReferenceNumber).HasColumnName("PY_REF_NUM");
        builder.Property(x => x.PayAmount).HasColumnName("PY_PAY_AMT").HasColumnType("DECIMAL(38,0)");
        builder.Property(x => x.OppositeAmount).HasColumnName("PY_OPP_AMT");
        builder.Property(x => x.EntryDate).HasColumnName("PY_ENT_DAT").HasPrecision(3);
        builder.Property(x => x.EnteredByUserId).HasColumnName("PY_ENT_USR");
        builder.Property(x => x.CancelFlag).HasColumnName("PY_CAN_FLG").HasColumnType("CHAR(1)");
        builder.Property(x => x.AttachmentNumber).HasColumnName("PY_ATT_NUM");
        builder.Property(x => x.CompanyCode).HasColumnName("PY_COM_COD").HasColumnType("CHAR(3)");
        builder.Property(x => x.EmployeeNumber).HasColumnName("PY_EMP_NUM");
        builder.Property(x => x.UpdateFlag).HasColumnName("PY_UPD_FLG").HasColumnType("CHAR(1)");
        builder.Property(x => x.SequenceNumber).HasColumnName("PY_SEQ_NUM");
        builder.Property(x => x.GradeType).HasColumnName("PY_GRD_TYP").HasColumnType("CHAR(3)");

        builder.Ignore(x => x.DomainEvents);
    }
}
