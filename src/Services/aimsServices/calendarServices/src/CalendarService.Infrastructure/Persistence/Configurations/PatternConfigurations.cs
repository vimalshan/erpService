using CalendarService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CalendarService.Infrastructure.Persistence.Configurations;

public class PatternMasterConfiguration : IEntityTypeConfiguration<PatternMaster>
{
    public void Configure(EntityTypeBuilder<PatternMaster> b)
    {
        b.ToTable("PATTERN_MASTER");
        b.HasKey(e => e.PatternId);
        b.Property(e => e.PatternId).HasColumnName("PATTERN_ID").ValueGeneratedNever();
        b.Property(e => e.PatternName).HasColumnName("PATTERN_NAME").HasMaxLength(100).IsRequired();
        b.Property(e => e.PatternDescription).HasColumnName("PATTERN_DESCRIPTION").HasMaxLength(255);
        b.Property(e => e.PatternCycleId).HasColumnName("PATTERN_CYCLEID").IsRequired();
        b.Property(e => e.LastModifiedBy).HasColumnName("PATTERN_LASTMODIFIEDBY").IsRequired();
        b.Property(e => e.LastModifiedOn).HasColumnName("PATTERN_LASTMODIFIEDON").IsRequired();

        b.HasIndex(e => e.PatternName).IsUnique().HasDatabaseName("UQ_PATTERN_NAME");
        b.HasIndex(e => e.PatternName).HasDatabaseName("IX_PATTERN_MASTER_NAME");

        b.HasMany(e => e.Details).WithOne(d => d.Pattern).HasForeignKey(d => d.PatDetPatternId);
    }
}

public class PatternDetailConfiguration : IEntityTypeConfiguration<PatternDetail>
{
    public void Configure(EntityTypeBuilder<PatternDetail> b)
    {
        b.ToTable("PATTERN_DETAIL");
        b.HasKey(e => e.PatDetId);
        b.Property(e => e.PatDetId).HasColumnName("PATDET_ID").ValueGeneratedNever();
        b.Property(e => e.PatDetPatternId).HasColumnName("PATDET_PATTERNID").IsRequired();
        b.Property(e => e.PatDetDayNo).HasColumnName("PATDET_DAYNO").IsRequired();
        b.Property(e => e.PatDetShiftId).HasColumnName("PATDET_SHIFTID").IsRequired();
        b.Property(e => e.LastModifiedBy).HasColumnName("PATDET_LASTMODIFIEDBY").IsRequired();
        b.Property(e => e.LastModifiedOn).HasColumnName("PATDET_LASTMODIFIEDON").IsRequired();

        b.HasOne(e => e.Shift).WithMany().HasForeignKey(e => e.PatDetShiftId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
