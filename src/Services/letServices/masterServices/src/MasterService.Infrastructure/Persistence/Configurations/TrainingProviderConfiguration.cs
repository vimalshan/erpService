using MasterService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MasterService.Infrastructure.Persistence.Configurations;

public class TrainingProviderConfiguration : IEntityTypeConfiguration<TrainingProvider>
{
    public void Configure(EntityTypeBuilder<TrainingProvider> builder)
    {
        builder.ToTable("TRAIN_MAST");
        builder.HasKey(t => t.TrainingCode);
        builder.Property(t => t.TrainingCode).HasColumnName("TR_TRN_COD").ValueGeneratedNever();
        builder.Property(t => t.TrainingName).HasColumnName("TR_TRN_NAM").HasMaxLength(255).IsRequired();
        builder.Property(t => t.Address1).HasColumnName("TR_TRN_ADD1").HasMaxLength(255);
        builder.Property(t => t.Address2).HasColumnName("TR_TRN_ADD2").HasMaxLength(255);
        builder.Property(t => t.Address3).HasColumnName("TR_TRN_ADD3").HasMaxLength(255);
        builder.Property(t => t.Address4).HasColumnName("TR_TRN_ADD4").HasMaxLength(255);
        builder.Property(t => t.ContactName1).HasColumnName("TR_CNT_NAM1").HasMaxLength(255);
        builder.Property(t => t.ContactName2).HasColumnName("TR_CNT_NAM2").HasMaxLength(255);
        builder.Property(t => t.Remark).HasColumnName("TR_REM_MRK").HasMaxLength(255);
        builder.Property(t => t.PhoneNum1).HasColumnName("TR_PHN_NUM1").HasMaxLength(255);
        builder.Property(t => t.PhoneNum2).HasColumnName("TR_PHN_NUM2").HasMaxLength(255);
        builder.Property(t => t.FaxNum1).HasColumnName("TR_FAX_NUM1").HasMaxLength(255);
        builder.Property(t => t.FaxNum2).HasColumnName("TR_FAX_NUM2").HasMaxLength(255);
        builder.Property(t => t.EmailAddress1).HasColumnName("TR_EML_ADD1").HasMaxLength(255);
        builder.Property(t => t.EmailAddress2).HasColumnName("TR_EML_ADD2").HasMaxLength(255);
        builder.Property(t => t.GroupCode).HasColumnName("TR_GRP_COD");
        builder.Property(t => t.VendorRating).HasColumnName("TR_VND_RAT").HasColumnType("decimal(38,0)");
        builder.Property(t => t.EffectiveDate).HasColumnName("TR_EFF_DAT");
        builder.Property(t => t.CancelDate).HasColumnName("TR_CAN_DAT");
        builder.Property(t => t.CancelRemark).HasColumnName("TR_CAN_REM").HasMaxLength(255);
        builder.Property(t => t.BrochureFilePath).HasColumnName("TR_BRC_FIL").HasMaxLength(255);
        builder.Property(t => t.VendorExpiry).HasColumnName("TR_VND_EXP").HasMaxLength(255);
        builder.Ignore(t => t.DomainEvents);
        builder.Ignore(t => t.IsActive);
    }
}
