using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Document.Domain.Entities;

namespace Document.Infrastructure.Persistence.Configurations;

public class AppraisalLetterConfiguration : IEntityTypeConfiguration<AppraisalLetter>
{
    public void Configure(EntityTypeBuilder<AppraisalLetter> builder)
    {
        builder.ToTable("DD_APPRAISALLETTER");
        builder.HasKey(l => l.SerialNo);
        builder.Property(l => l.SerialNo).HasColumnName("DD_SRL_NO").HasColumnType("decimal(38,0)");
        builder.Property(l => l.BandCode).HasColumnName("DD_APR_BND").HasColumnType("decimal(38,0)");
        builder.Property(l => l.LetterType).HasColumnName("DD_APR_TYP").HasMaxLength(3);
        builder.Property(l => l.FromDate).HasColumnName("DD_APR_FRM");
        builder.Property(l => l.EndDate).HasColumnName("DD_APR_END");
        builder.Property(l => l.Paragraph1).HasColumnName("DD_APR_PR1").HasMaxLength(1000);
        builder.Property(l => l.Paragraph2).HasColumnName("DD_APR_PR2").HasMaxLength(1000);
        builder.Property(l => l.Paragraph3).HasColumnName("DD_APR_PR3").HasMaxLength(1000);
        builder.Property(l => l.Paragraph4).HasColumnName("DD_APR_PR4").HasMaxLength(1000);
        builder.Property(l => l.Paragraph5).HasColumnName("DD_APR_PR5").HasMaxLength(1000);
        builder.Property(l => l.EffectiveDate).HasColumnName("DD_EFF_DAT");
        builder.Property(l => l.BasicPayEffectiveDate).HasColumnName("DD_BAS_DAT");
        builder.Property(l => l.PrintDate).HasColumnName("DD_PRN_DAT");
        builder.Ignore(l => l.DomainEvents);
    }
}
