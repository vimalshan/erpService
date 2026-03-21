using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourPlanService.Domain.Entities;

namespace TourPlanService.Infrastructure.Data.Configurations;

public sealed class InternationalScheduleConfiguration : IEntityTypeConfiguration<InternationalSchedule>
{
    public void Configure(EntityTypeBuilder<InternationalSchedule> builder)
    {
        builder.ToTable("TOURPLAN_INTSCH");
        builder.HasKey(x => x.IntSchId);
        builder.Property(x => x.IntSchId).HasColumnName("INTSCH_ID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.IntSchTpId).HasColumnName("INTSCH_TPID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.IntSchFromDate).HasColumnName("INTSCH_FROMDATE").IsRequired();
        builder.Property(x => x.IntSchFromTime).HasColumnName("INTSCH_FROMTIME").HasMaxLength(255).IsRequired();
        builder.Property(x => x.IntSchFromCityId).HasColumnName("INTSCH_FROMCITYID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.IntSchFromCity).HasColumnName("INTSCH_FROMCITY").HasMaxLength(255).IsRequired();
        builder.Property(x => x.IntSchFromCountry).HasColumnName("INTSCH_FROMCOUNTRY").HasMaxLength(255).IsRequired();
        builder.Property(x => x.IntSchToDate).HasColumnName("INTSCH_TODATE").IsRequired();
        builder.Property(x => x.IntSchToTime).HasColumnName("INTSCH_TOTIME").HasMaxLength(255).IsRequired();
        builder.Property(x => x.IntSchToCityId).HasColumnName("INTSCH_TOCITYID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.IntSchToCity).HasColumnName("INTSCH_TOCITY").HasMaxLength(255).IsRequired();
        builder.Property(x => x.IntSchToCountry).HasColumnName("INTSCH_TOCOUNTRY").HasMaxLength(255).IsRequired();
        builder.Property(x => x.IntSchApproxCost).HasColumnName("INTSCH_APPROXCOST").HasMaxLength(255).IsRequired();
        builder.Ignore(x => x.DomainEvents);
    }
}

public sealed class TourLeaveConfiguration : IEntityTypeConfiguration<TourLeave>
{
    public void Configure(EntityTypeBuilder<TourLeave> builder)
    {
        builder.ToTable("TOURPLAN_LEAVE");
        builder.HasKey(x => x.LeaveTpLeaveId);
        builder.Property(x => x.LeaveTpLeaveId).HasColumnName("LEAVE_TPLEAVEID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.LeaveTpId).HasColumnName("LEAVE_TPID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.LeaveFromDate).HasColumnName("LEAVE_FROMDATE").IsRequired();
        builder.Property(x => x.LeaveToDate).HasColumnName("LEAVE_TODATE").IsRequired();
        builder.Property(x => x.LeaveFromSession).HasColumnName("LEAVE_FROMSESSION").HasMaxLength(255).IsRequired();
        builder.Property(x => x.LeaveToSession).HasColumnName("LEAVE_TOSESSION").HasMaxLength(255).IsRequired();
        builder.Property(x => x.LeaveType).HasColumnName("LEAVE_TYPE").HasMaxLength(255).IsRequired();
        builder.Property(x => x.LeaveDays).HasColumnName("LEAVE_DAYS").HasMaxLength(255).IsRequired();
        builder.Property(x => x.LeaveRemarks).HasColumnName("LEAVE_REMARKS").HasMaxLength(255).IsRequired();
        builder.Property(x => x.LeaveId).HasColumnName("LEAVE_ID").HasMaxLength(255).IsRequired();
        builder.Ignore(x => x.DomainEvents);
    }
}

public sealed class NmsScheduleConfiguration : IEntityTypeConfiguration<NmsSchedule>
{
    public void Configure(EntityTypeBuilder<NmsSchedule> builder)
    {
        builder.ToTable("TOURPLAN_NMSSCH");
        builder.HasKey(x => x.NmsSchId);
        builder.Property(x => x.NmsSchId).HasColumnName("NMSSCH_ID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.NmsSchTpId).HasColumnName("NMSSCH_TPID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.NmsSchCityId).HasColumnName("NMSSCH_CITYID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.NmsSchCityName).HasColumnName("NMSSCH_CITYNAME").HasMaxLength(255).IsRequired();
        builder.Property(x => x.NmsSchFromDate).HasColumnName("NMSSCH_FROMDATE").IsRequired();
        builder.Property(x => x.NmsSchFromTime).HasColumnName("NMSSCH_FROMTIME").HasMaxLength(255).IsRequired();
        builder.Property(x => x.NmsSchToDate).HasColumnName("NMSSCH_TODATE").IsRequired();
        builder.Property(x => x.NmsSchToTime).HasColumnName("NMSSCH_TOTIME").HasMaxLength(255).IsRequired();
        builder.Property(x => x.NmsSchNoDays).HasColumnName("NMSSCH_NODAYS").HasMaxLength(255).IsRequired();
        builder.Property(x => x.NmsSchModeId).HasColumnName("NMSSCH_MODEID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.NmsSchClassId).HasColumnName("NMSSCH_CLASSID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.NmsSchPurpose).HasColumnName("NMSSCH_PURPOSE").HasMaxLength(255).IsRequired();
        builder.Property(x => x.NmsSchRemarks).HasColumnName("NMSSCH_REMARKS").HasMaxLength(255).IsRequired();
        builder.Ignore(x => x.DomainEvents);
    }
}

public sealed class SelfExpenseConfiguration : IEntityTypeConfiguration<SelfExpense>
{
    public void Configure(EntityTypeBuilder<SelfExpense> builder)
    {
        builder.ToTable("TOURPLAN_SLFEXP");
        builder.HasKey(x => x.ExpTktId);
        builder.Property(x => x.ExpTktId).HasColumnName("EXP_TKTID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ExpTpId).HasColumnName("EXP_TPID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ExpExpCat).HasColumnName("EXP_EXPCAT").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ExpTravelMode).HasColumnName("EXP_TRAVELMODE").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ExpFromDate).HasColumnName("EXP_FROMDATE").IsRequired();
        builder.Property(x => x.ExpFromCity).HasColumnName("EXP_FROMCITY").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ExpFromCityName).HasColumnName("EXP_FROMCITYNAME").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ExpToDate).HasColumnName("EXP_TODATE").IsRequired();
        builder.Property(x => x.ExpToCity).HasColumnName("EXP_TOCITY").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ExpToCityName).HasColumnName("EXP_TOCITYNAME").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ExpNoOfDays).HasColumnName("EXP_NOOFDAYS").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ExpEntitleValue).HasColumnName("EXP_ENTITLEVALUE").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ExpValue).HasColumnName("EXP_VALUE").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ExpSerTaxVal).HasColumnName("EXP_SERTAXVAL").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ExpAdlValue).HasColumnName("EXP_ADLVALUE").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ExpTravelClass).HasColumnName("EXP_TRAVELCLASS").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ExpRemarks).HasColumnName("EXP_REMARKS").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ExpApprovedAmt).HasColumnName("EXP_APPROVEDAMT").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ExpFinRemarks).HasColumnName("EXP_FINREMARKS").HasMaxLength(255);
        builder.Property(x => x.ExpExpId).HasColumnName("EXP_EXPID").HasMaxLength(255).IsRequired();
        builder.Ignore(x => x.DomainEvents);
    }
}
