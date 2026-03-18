using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Document.Domain.Entities;

namespace Document.Infrastructure.Persistence.Configurations;

public class Annexure1Configuration : IEntityTypeConfiguration<Annexure1>
{
    public void Configure(EntityTypeBuilder<Annexure1> builder)
    {
        builder.ToTable("DD_ANNEXURE1");
        builder.HasNoKey();
        builder.Property(a => a.CreatedByPin).HasColumnName("DD_CRT_PIN").HasColumnType("decimal(38,0)");
        builder.Property(a => a.UserPin).HasColumnName("DD_USR_PIN").HasColumnType("decimal(38,0)");
        builder.Property(a => a.UserName).HasColumnName("DD_USR_NAM").HasMaxLength(150);
        builder.Property(a => a.Answer1).HasColumnName("DD_AN1_PR1").HasMaxLength(1000);
        builder.Property(a => a.Answer2).HasColumnName("DD_AN2_PR2").HasMaxLength(1000);
        builder.Property(a => a.Answer3).HasColumnName("DD_AN3_PR3").HasMaxLength(1000);
        builder.Property(a => a.Answer4).HasColumnName("DD_AN4_PR4").HasMaxLength(1000);
        builder.Property(a => a.SignatoryName).HasColumnName("DD_SIG_NAM").HasMaxLength(150);
        builder.Property(a => a.SignatoryDesignation).HasColumnName("DD_SIG_DSG").HasMaxLength(100);
        builder.Property(a => a.UserRandomNumber).HasColumnName("DD_USR_RNM").HasMaxLength(150);
        builder.Property(a => a.UserUnitCode).HasColumnName("DD_USR_UNT").HasMaxLength(50);
        builder.Property(a => a.PrintDate).HasColumnName("DD_PRN_DAT");
        builder.Property(a => a.AppraisalLumpsum).HasColumnName("DD_APR_LMP").HasColumnType("decimal(38,0)");
        builder.Property(a => a.AppraisalBasicPay).HasColumnName("DD_APR_BAS").HasColumnType("decimal(38,0)");
        builder.Property(a => a.AppraisalFlexiPay).HasColumnName("DD_APR_FLX").HasColumnType("decimal(38,0)");
        builder.Property(a => a.EffectiveDate).HasColumnName("DD_EFF_DAT");
        builder.Ignore(a => a.DomainEvents);
    }
}

public class Annexure2Configuration : IEntityTypeConfiguration<Annexure2>
{
    public void Configure(EntityTypeBuilder<Annexure2> builder)
    {
        builder.ToTable("DD_ANNEXURE2");
        builder.HasNoKey();
        builder.Property(a => a.CreatedByPin).HasColumnName("DD_CRT_PIN").HasColumnType("decimal(38,0)");
        builder.Property(a => a.UserPin).HasColumnName("DD_USR_PIN").HasColumnType("decimal(38,0)");
        builder.Property(a => a.UserName).HasColumnName("DD_USR_NAM").HasMaxLength(150);
        builder.Property(a => a.BasicOld).HasColumnName("DD_BAS_OLD").HasColumnType("decimal(38,0)");
        builder.Property(a => a.BasicNew).HasColumnName("DD_BAS_NEW").HasColumnType("decimal(38,0)");
        builder.Property(a => a.FlexiPay).HasColumnName("DD_FLX_PAY").HasColumnType("decimal(38,0)");
        builder.Property(a => a.SignatoryName).HasColumnName("DD_SIG_NAM").HasMaxLength(150);
        builder.Property(a => a.SignatoryDesignation).HasColumnName("DD_SIG_DSG").HasMaxLength(100);
        builder.Property(a => a.EffectiveDate).HasColumnName("DD_EFF_DAT");
        builder.Property(a => a.PrintDate).HasColumnName("DD_PRN_DAT").HasMaxLength(20);
        builder.Property(a => a.BandName).HasColumnName("DD_BND_NAM").HasMaxLength(1);
        builder.Ignore(a => a.DomainEvents);
    }
}
