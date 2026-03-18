using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimeAttendance.Domain.Entities;

namespace TimeAttendance.Infrastructure.Persistence.Configurations;

public class AbsenteeismMisConfiguration : IEntityTypeConfiguration<AbsenteeismMis>
{
    public void Configure(EntityTypeBuilder<AbsenteeismMis> builder)
    {
        builder.ToTable("ABSMIS");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("ABSID").ValueGeneratedOnAdd();

        builder.Property(x => x.UnitId).HasColumnName("UNTID").IsRequired(false);
        builder.Property(x => x.CompanyId).HasColumnName("CID").IsRequired(false);
        builder.Property(x => x.DepartmentId).HasColumnName("DID").IsRequired(false);
        builder.Property(x => x.SystemId).HasColumnName("SYSID").IsRequired(false);
        builder.Property(x => x.Grade).HasColumnName("GRD").HasMaxLength(3).IsFixedLength().IsRequired(false);
        builder.Property(x => x.PlannedLeave).HasColumnName("PLD").HasColumnType("DECIMAL(38,2)").IsRequired(false);
        builder.Property(x => x.PaidDays).HasColumnName("PDS").HasColumnType("DECIMAL(38,2)").IsRequired(false);
        builder.Property(x => x.WeeklyOff).HasColumnName("WOFF").HasColumnType("DECIMAL(38,2)").IsRequired(false);
        builder.Property(x => x.LeaveWithoutPay).HasColumnName("LWOP").HasColumnType("DECIMAL(38,2)").IsRequired(false);
        builder.Property(x => x.NumberOfPresentHours).HasColumnName("NPH").HasColumnType("DECIMAL(38,2)").IsRequired(false);
        builder.Property(x => x.CompensatoryOff).HasColumnName("COF").HasColumnType("DECIMAL(38,2)").IsRequired(false);
        builder.Property(x => x.BankLeave).HasColumnName("BKL").HasColumnType("DECIMAL(38,2)").IsRequired(false);
        builder.Property(x => x.AnnualPaidLeave).HasColumnName("APL").HasColumnType("DECIMAL(38,2)").IsRequired(false);
        builder.Property(x => x.PenaltyLeave).HasColumnName("PNL").HasColumnType("DECIMAL(38,2)").IsRequired(false);
        builder.Property(x => x.ShiftSwap).HasColumnName("SWP").HasColumnType("DECIMAL(38,2)").IsRequired(false);
        builder.Property(x => x.OnDuty).HasColumnName("OND").HasColumnType("DECIMAL(38,2)").IsRequired(false);
        builder.Property(x => x.Month).HasColumnName("MNTH").HasMaxLength(6).IsRequired(false);
        builder.Property(x => x.LogSystemId).HasColumnName("LOGSYSID").HasColumnType("DECIMAL(38,2)").IsRequired(false);
        builder.Property(x => x.LeaveWithoutPayPercentage).HasColumnName("LWOPP").HasColumnType("DECIMAL(38,2)").IsRequired(false);

        builder.Property(x => x.CreatedAt).HasColumnName("CREATED_AT").HasDefaultValueSql("GETUTCDATE()");
        builder.Property(x => x.CreatedBy).HasColumnName("CREATED_BY").HasMaxLength(100).HasDefaultValue(string.Empty);
        builder.Property(x => x.LastModifiedAt).HasColumnName("LAST_MODIFIED_AT").IsRequired(false);
        builder.Property(x => x.LastModifiedBy).HasColumnName("LAST_MODIFIED_BY").HasMaxLength(100).IsRequired(false);

        builder.Ignore(x => x.DomainEvents);

        builder.HasIndex(x => new { x.UnitId, x.Month })
            .HasDatabaseName("IX_ABSMIS_UNIT_MONTH");
    }
}
