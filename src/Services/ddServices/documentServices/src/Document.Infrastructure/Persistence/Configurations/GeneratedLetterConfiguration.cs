using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Document.Domain.Entities;

namespace Document.Infrastructure.Persistence.Configurations;

public class GeneratedLetterConfiguration : IEntityTypeConfiguration<GeneratedLetter>
{
    public void Configure(EntityTypeBuilder<GeneratedLetter> builder)
    {
        builder.ToTable("DD_GENERATELETTER");
        builder.HasNoKey();
        builder.Property(l => l.CreatedByPin).HasColumnName("DD_CRT_PIN").HasColumnType("decimal(38,0)");
        builder.Property(l => l.EmployeePin).HasColumnName("DD_USR_PIN").HasColumnType("decimal(38,0)");
        builder.Property(l => l.EmployeeName).HasColumnName("DD_USR_NAM").HasMaxLength(150);
        builder.Property(l => l.SignatoryName).HasColumnName("DD_SIG_NAM").HasMaxLength(150);
        builder.Property(l => l.SignatoryDesignation).HasColumnName("DD_SIG_DSG").HasMaxLength(100);
        builder.Property(l => l.EmployeeRandomNumber).HasColumnName("DD_USR_RNM").HasMaxLength(150);
        builder.Property(l => l.EmployeeUnitCode).HasColumnName("DD_USR_UNT").HasMaxLength(50);
        builder.Property(l => l.PrintDate).HasColumnName("DD_PRN_DAT");
        builder.Property(l => l.AppraisalLumpsum).HasColumnName("DD_APR_LMP").HasColumnType("decimal(38,0)");
        builder.Property(l => l.AppraisalBasicPay).HasColumnName("DD_APR_BAS").HasColumnType("decimal(38,0)");
        builder.Property(l => l.AppraisalFlexiPay).HasColumnName("DD_APR_FLX").HasColumnType("decimal(38,0)");
        builder.Property(l => l.EffectiveDate).HasColumnName("DD_EFF_DAT");
        builder.Property(l => l.LetterType).HasColumnName("DD_LETTERTYPE").HasMaxLength(10);
        builder.Property(l => l.FinalRating).HasColumnName("DD_FINALRATING").HasMaxLength(5);
        builder.Property(l => l.AppraisalIncrement).HasColumnName("DD_APR_INC").HasColumnType("decimal(38,0)");
        builder.Property(l => l.PromotionLevel).HasColumnName("DD_PRM_LEVEL").HasColumnType("decimal(38,0)");
        builder.Property(l => l.AppraisalDesignation).HasColumnName("DD_APR_DSG").HasMaxLength(100);
        builder.Property(l => l.AppraisalBand).HasColumnName("DD_APR_BND").HasMaxLength(50);
        builder.Property(l => l.SignatoryName2).HasColumnName("DD_SIG_NAM2").HasMaxLength(150);
        builder.Property(l => l.SignatoryDesignation2).HasColumnName("DD_SIG_DSG2").HasMaxLength(100);
        builder.Property(l => l.IncrementTemplateId).HasColumnName("DD_INC_TEMPID").HasColumnType("decimal(38,0)");
        builder.Property(l => l.RatingTemplateId).HasColumnName("DD_RAT_TEMPID").HasColumnType("decimal(38,0)");
        builder.Ignore(l => l.DomainEvents);
    }
}
