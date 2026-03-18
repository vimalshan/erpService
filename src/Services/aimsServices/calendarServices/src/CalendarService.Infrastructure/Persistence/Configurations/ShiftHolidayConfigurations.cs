using CalendarService.Domain.Entities;
using CalendarService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CalendarService.Infrastructure.Persistence.Configurations;

public class HolidayMasterConfiguration : IEntityTypeConfiguration<HolidayMaster>
{
    public void Configure(EntityTypeBuilder<HolidayMaster> b)
    {
        b.ToTable("HOLIDAY_MASTER");
        b.HasKey(e => e.HolidayId);
        b.Property(e => e.HolidayId).HasColumnName("HOLIDAY_ID").ValueGeneratedNever();
        b.Property(e => e.HolidayDate).HasColumnName("HOLIDAY_DATE").IsRequired();
        b.Property(e => e.HolidayDescription).HasColumnName("HOLIDAY_DESCRIPTION").HasMaxLength(255).IsRequired();
        b.Property(e => e.HolidayType).HasColumnName("HOLIDAY_TYPE")
            .HasConversion(v => ((char)(int)v).ToString(), v => (HolidayType)v[0]);
        b.Property(e => e.HolidayUnit).HasColumnName("HOLIDAY_UNIT");
        b.Property(e => e.LastModifiedBy).HasColumnName("HOLIDAY_LASTMODIFIEDBY").IsRequired();
        b.Property(e => e.LastModifiedOn).HasColumnName("HOLIDAY_LASTMODIFIEDON").IsRequired();

        b.HasIndex(e => e.HolidayDate).HasDatabaseName("IX_HOLIDAY_MASTER_DATE");
    }
}

public class ShiftMasterConfiguration : IEntityTypeConfiguration<ShiftMaster>
{
    public void Configure(EntityTypeBuilder<ShiftMaster> b)
    {
        b.ToTable("SHIFT_MASTER");
        b.HasKey(e => e.ShiftId);
        b.Property(e => e.ShiftId).HasColumnName("SHIFT_ID").ValueGeneratedNever();
        b.Property(e => e.ShiftCode).HasColumnName("SHIFT_CODE").HasMaxLength(20).IsRequired();
        b.Property(e => e.ShiftName).HasColumnName("SHIFT_NAME").HasMaxLength(255).IsRequired();
        b.Property(e => e.ShiftInTime).HasColumnName("SHIFT_INTIME").IsRequired();
        b.Property(e => e.ShiftOutTime).HasColumnName("SHIFT_OUTTIME").IsRequired();
        b.Property(e => e.ShiftDuration).HasColumnName("SHIFT_DURATION").HasColumnType("DECIMAL(5,2)").IsRequired();
        b.Property(e => e.LastModifiedBy).HasColumnName("SHIFT_LASTMODIFIEDBY").IsRequired();
        b.Property(e => e.LastModifiedOn).HasColumnName("SHIFT_LASTMODIFIEDON").IsRequired();

        b.HasIndex(e => e.ShiftCode).IsUnique().HasDatabaseName("UQ_SHIFT_CODE");
        b.HasIndex(e => e.ShiftCode).HasDatabaseName("IX_SHIFT_MASTER_CODE");

        b.HasMany(e => e.TimeMasters).WithOne(t => t.Shift).HasForeignKey(t => t.ShiftTimeShiftId);
        b.HasMany(e => e.Exceptions).WithOne(t => t.Shift).HasForeignKey(t => t.ShiftExcShiftId);
    }
}

public class ShiftTimeMasterConfiguration : IEntityTypeConfiguration<ShiftTimeMaster>
{
    public void Configure(EntityTypeBuilder<ShiftTimeMaster> b)
    {
        b.ToTable("SHIFT_TIMEMASTER");
        b.HasKey(e => e.ShiftTimeId);
        b.Property(e => e.ShiftTimeId).HasColumnName("SHIFTTIME_ID").ValueGeneratedNever();
        b.Property(e => e.ShiftTimeShiftId).HasColumnName("SHIFTTIME_SHIFTID").IsRequired();
        b.Property(e => e.ShiftTimeInTime).HasColumnName("SHIFTTIME_INTIME").IsRequired();
        b.Property(e => e.ShiftTimeOutTime).HasColumnName("SHIFTTIME_OUTTIME").IsRequired();
        b.Property(e => e.LastModifiedBy).HasColumnName("SHIFTTIME_LASTMODIFIEDBY").IsRequired();
        b.Property(e => e.LastModifiedOn).HasColumnName("SHIFTTIME_LASTMODIFIEDON").IsRequired();
    }
}

public class ShiftExceptionConfiguration : IEntityTypeConfiguration<Domain.Entities.ShiftException>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.ShiftException> b)
    {
        b.ToTable("SHIFT_EXCEPTION");
        b.HasKey(e => e.ShiftExcId);
        b.Property(e => e.ShiftExcId).HasColumnName("SHIFTEXC_ID").ValueGeneratedNever();
        b.Property(e => e.ShiftExcShiftId).HasColumnName("SHIFTEXC_SHIFTID").IsRequired();
        b.Property(e => e.ShiftExcEffDate).HasColumnName("SHIFTEXC_EFFDATE").IsRequired();
        b.Property(e => e.ShiftExcClsDate).HasColumnName("SHIFTEXC_CLSDATE");
        b.Property(e => e.ShiftExcNewShiftId).HasColumnName("SHIFTEXC_NEWSHIFTID").IsRequired();
        b.Property(e => e.LastModifiedBy).HasColumnName("SHIFTEXC_LASTMODIFIEDBY").IsRequired();
        b.Property(e => e.LastModifiedOn).HasColumnName("SHIFTEXC_LASTMODIFIEDON").IsRequired();

        b.HasIndex(e => e.ShiftExcShiftId).HasDatabaseName("IX_SHIFTEXC_SHIFTID");

        b.HasOne(e => e.NewShift).WithMany().HasForeignKey(e => e.ShiftExcNewShiftId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
