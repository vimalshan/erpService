using CalendarService.Domain.Entities;
using CalendarService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CalendarService.Infrastructure.Persistence.Configurations;

public class CalendarMasterConfiguration : IEntityTypeConfiguration<CalendarMaster>
{
    public void Configure(EntityTypeBuilder<CalendarMaster> b)
    {
        b.ToTable("CALENDAR_MASTER");
        b.HasKey(e => e.CalendarId);
        b.Property(e => e.CalendarId).HasColumnName("CALENDAR_ID").ValueGeneratedNever();
        b.Property(e => e.CalendarName).HasColumnName("CALENDAR_NAME").HasMaxLength(255).IsRequired();
        b.Property(e => e.CalendarUnitId).HasColumnName("CALENDAR_UNITID").IsRequired();
        b.Property(e => e.CalendarEffDate).HasColumnName("CALENDAR_EFFDATE").IsRequired();
        b.Property(e => e.CalendarClsDate).HasColumnName("CALENDAR_CLSDATE");
        b.Property(e => e.Status).HasColumnName("CALENDAR_STATUS")
            .HasConversion(v => ((char)(int)v).ToString(), v => (CalendarStatus)v[0]);
        b.Property(e => e.LastModifiedBy).HasColumnName("CALENDAR_LASTMODIFIEDBY").IsRequired();
        b.Property(e => e.LastModifiedOn).HasColumnName("CALENDAR_LASTMODIFIEDON").IsRequired();

        b.HasIndex(e => e.CalendarName).IsUnique().HasDatabaseName("UQ_CALENDAR_NAME");
        b.HasIndex(e => e.Status).HasDatabaseName("IX_CALENDAR_MASTER_STATUS");

        b.HasMany(e => e.UnitMaps).WithOne(u => u.Calendar).HasForeignKey(u => u.CalUnitCalenId);
        b.HasMany(e => e.RoundRanges).WithOne(r => r.Calendar).HasForeignKey(r => r.CalRoundCalenId);
        b.HasMany(e => e.GraceRanges).WithOne(g => g.Calendar).HasForeignKey(g => g.CalGraceCalenId);
    }
}

public class CalendarUnitMapConfiguration : IEntityTypeConfiguration<CalendarUnitMap>
{
    public void Configure(EntityTypeBuilder<CalendarUnitMap> b)
    {
        b.ToTable("CALENDAR_UNITMAP");
        b.HasKey(e => e.CalUnitId);
        b.Property(e => e.CalUnitId).HasColumnName("CALUNIT_ID").ValueGeneratedNever();
        b.Property(e => e.CalUnitCalenId).HasColumnName("CALUNIT_CALENID").IsRequired();
        b.Property(e => e.CalUnitUnitId).HasColumnName("CALUNIT_UNITID").IsRequired();
        b.Property(e => e.CalUnitEffDate).HasColumnName("CALUNIT_EFFDATE").IsRequired();
        b.Property(e => e.CalUnitClsDate).HasColumnName("CALUNIT_CLSDATE");
        b.Property(e => e.LastModifiedBy).HasColumnName("CALUNIT_LASTMODIFIEDBY").IsRequired();
        b.Property(e => e.LastModifiedOn).HasColumnName("CALUNIT_LASTMODIFIEDON").IsRequired();
    }
}

public class CalendarRoundRangeConfiguration : IEntityTypeConfiguration<CalendarRoundRange>
{
    public void Configure(EntityTypeBuilder<CalendarRoundRange> b)
    {
        b.ToTable("CALENDAR_ROUNDRANGE");
        b.HasKey(e => e.CalRoundId);
        b.Property(e => e.CalRoundId).HasColumnName("CALROUND_ID").ValueGeneratedNever();
        b.Property(e => e.CalRoundCalenId).HasColumnName("CALROUND_CALENID").IsRequired();
        b.Property(e => e.CalRoundRoundNo).HasColumnName("CALROUND_ROUNDNO").IsRequired();
        b.Property(e => e.CalRoundRoundFrom).HasColumnName("CALROUND_ROUNDFROM").IsRequired();
        b.Property(e => e.CalRoundRoundTo).HasColumnName("CALROUND_ROUNDTO").IsRequired();
        b.Property(e => e.CalRoundWorking).HasColumnName("CALROUND_WORKING").IsRequired();
        b.Property(e => e.LastModifiedBy).HasColumnName("CALROUND_LASTMODIFIEDBY").IsRequired();
        b.Property(e => e.LastModifiedOn).HasColumnName("CALROUND_LASTMODIFIEDON").IsRequired();
    }
}

public class CalendarGraceRangeConfiguration : IEntityTypeConfiguration<CalendarGraceRange>
{
    public void Configure(EntityTypeBuilder<CalendarGraceRange> b)
    {
        b.ToTable("CALENDAR_GRACERANGE");
        b.HasKey(e => e.CalGraceId);
        b.Property(e => e.CalGraceId).HasColumnName("CALGRACE_ID").ValueGeneratedNever();
        b.Property(e => e.CalGraceCalenId).HasColumnName("CALGRACE_CALENID").IsRequired();
        b.Property(e => e.CalGraceGraceId).HasColumnName("CALGRACE_GRACEID").IsRequired();
        b.Property(e => e.CalGraceGraceTime).HasColumnName("CALGRACE_GRACETIME").IsRequired();
        b.Property(e => e.LastModifiedBy).HasColumnName("CALGRACE_LASTMODIFIEDBY").IsRequired();
        b.Property(e => e.LastModifiedOn).HasColumnName("CALGRACE_LASTMODIFIEDON").IsRequired();
    }
}
