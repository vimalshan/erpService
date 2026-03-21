using FinanceService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceService.Infrastructure.Persistence.Configurations;

public class JvPostingDetailConfiguration : IEntityTypeConfiguration<JvPostingDetail>
{
    public void Configure(EntityTypeBuilder<JvPostingDetail> builder)
    {
        builder.ToTable("JVPOSTDET");
        builder.HasKey(e => e.JvIntCode);
        builder.Property(e => e.JvIntCode).HasColumnName("JVINTCODE").ValueGeneratedNever();
        builder.Property(e => e.JvDocNum).HasColumnName("JVDOCNUM");
        builder.Property(e => e.CompanyCode).HasColumnName("JV_COM_COD").HasMaxLength(3).IsFixedLength();
        builder.Property(e => e.GradeType).HasColumnName("JV_GRD_TYP").HasMaxLength(3);
        builder.Property(e => e.StartDate).HasColumnName("JV_ST_DAT");
        builder.Property(e => e.EndDate).HasColumnName("JV_ED_DAT");
        builder.Property(e => e.Comment).HasColumnName("JV_COMMENT").HasMaxLength(50);
        builder.Property(e => e.Status).HasColumnName("JV_STATUS");
        builder.Property(e => e.PayNumber).HasColumnName("JV_PAY_NUM");
        builder.Property(e => e.JvDate).HasColumnName("JV_DATE");
    }
}
