using HealthTransaction.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthTransaction.Infrastructure.Persistence.Configurations;

public class CheckupCardConfiguration : IEntityTypeConfiguration<CheckupCard>
{
    public void Configure(EntityTypeBuilder<CheckupCard> builder)
    {
        builder.ToTable("HLTH_CHKUP_CARD");
        builder.HasKey(e => e.HlthNum);

        builder.Property(e => e.HlthNum).HasColumnName("HCC_HLTH_NUM").HasColumnType("NUMERIC(10,0)");
        builder.Property(e => e.EmpNum).HasColumnName("HCC_EMP_NUM").HasColumnType("NUMERIC(10,0)");
        builder.Property(e => e.EmpDate).HasColumnName("HCC_EMP_DATE").HasColumnType("DATE");
        builder.Property(e => e.ComCode).HasColumnName("HCC_COM_COD").HasMaxLength(3);
        builder.Property(e => e.PersonalDetails).HasColumnName("HCC_PER_DET").HasMaxLength(1000);
        builder.Property(e => e.ComplaintDetails).HasColumnName("HCC_COMPL_DET").HasMaxLength(1000);
        builder.Property(e => e.AdvRemark1).HasColumnName("HCC_ADV_RMK1").HasMaxLength(500);
        builder.Property(e => e.AdvRemark2).HasColumnName("HCC_ADV_RMK2").HasMaxLength(500);
        builder.Property(e => e.DocDate1).HasColumnName("HCC_DOC_DATE1").HasColumnType("DATE");
        builder.Property(e => e.DocDate2).HasColumnName("HCC_DOC_DATE2").HasColumnType("DATE");
        builder.Property(e => e.AdvFollow1).HasColumnName("HCC_ADV_FOLLOW1").HasMaxLength(500);
        builder.Property(e => e.AdvFollow2).HasColumnName("HCC_ADV_FOLLOW2").HasMaxLength(500);

        builder.HasMany(e => e.SubRecords)
               .WithOne(s => s.CheckupCard)
               .HasForeignKey(s => s.HlthNum)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
