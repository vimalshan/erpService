using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelService.Domain.Entities.TourPlan;
using TravelService.Domain.ValueObjects;

namespace TravelService.Infrastructure.Persistence.Configurations;

public class TourPlanConfiguration : IEntityTypeConfiguration<TourPlan>
{
    public void Configure(EntityTypeBuilder<TourPlan> builder)
    {
        builder.ToTable("TOURPLAN_MAIN");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("TP_ID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.EmployeeSysId).HasColumnName("TP_EMPSYSID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.StartDate).HasColumnName("TP_STARTDATE").IsRequired();
        builder.Property(x => x.EndDate).HasColumnName("TP_ENDDATE");
        builder.Property(x => x.Purpose).HasColumnName("TP_PURPOSE").HasMaxLength(255).IsRequired();
        builder.Property(x => x.Remarks).HasColumnName("TP_REMARKS").HasMaxLength(255).IsRequired();
        builder.Property(x => x.Status).HasColumnName("TP_STATUS").HasMaxLength(255).IsRequired();
        builder.Property(x => x.Category).HasColumnName("TP_CATEGORY").HasMaxLength(255).IsRequired();
        builder.Property(x => x.IncludeBookingRequests).HasColumnName("TP_BOOKINC").HasConversion<string>().HasMaxLength(255).IsRequired();
        builder.Property(x => x.TripType).HasColumnName("TP_TYPE").HasMaxLength(255);
        builder.Property(x => x.CreatedBy).HasColumnName("TP_CREATEDBY").HasMaxLength(255).IsRequired();
        builder.Property(x => x.CreatedOn).HasColumnName("TP_CREATEDON").IsRequired();
        builder.Property(x => x.ApprovedBy).HasColumnName("TP_APPROVEDBY").HasMaxLength(255);
        builder.Property(x => x.ApprovedOn).HasColumnName("TP_APPROVEDON");
        builder.Property(x => x.LastModifiedBy).HasColumnName("TP_LASTMODIFIEDBY").HasMaxLength(255).IsRequired();
        builder.Property(x => x.LastModifiedOn).HasColumnName("TP_LASTMODIFIEDON").IsRequired();
        builder.Property(x => x.SupervisorRemarks).HasColumnName("TP_SUPREMARKS").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ContactNo).HasColumnName("TP_CONTACTNO").HasMaxLength(255);
        builder.Property(x => x.GradeType).HasColumnName("TP_GRADETYPE").HasMaxLength(255);
        builder.Property(x => x.PayrollUnitId).HasColumnName("TP_PAYUNITID").HasMaxLength(255);
        builder.Property(x => x.ClaimType).HasColumnName("TP_CLAIMTYPE").HasMaxLength(255);
        builder.Property(x => x.ApproverRemarks).HasColumnName("TP_APPREMARKS").HasMaxLength(255);
        builder.Property(x => x.ExpenseStatus).HasColumnName("TP_EXPSTATUS").HasMaxLength(255);
        builder.Property(x => x.ClosureStatus).HasColumnName("TP_CLOSURESTATUS").HasMaxLength(1);
        builder.Property(x => x.ActualOutcome).HasColumnName("TP_ACTREMARKS").HasMaxLength(255);

        builder.OwnsOne(x => x.FromCity, oc =>
        {
            oc.Property(c => c.CityId).HasColumnName("TP_FROMCITYID").HasMaxLength(255).IsRequired();
            oc.Property(c => c.CityName).HasColumnName("TP_FROMCITYNAME").HasMaxLength(255).IsRequired();
            oc.Ignore(c => c.CountryId);
            oc.Ignore(c => c.CountryName);
        });
        builder.OwnsOne(x => x.ToCity, oc =>
        {
            oc.Property(c => c.CityId).HasColumnName("TP_TOCITYID").HasMaxLength(255).IsRequired();
            oc.Property(c => c.CityName).HasColumnName("TP_TOCITYNAME").HasMaxLength(255).IsRequired();
            oc.Ignore(c => c.CountryId);
            oc.Ignore(c => c.CountryName);
        });

        builder.HasMany(x => x.Advances).WithOne().HasForeignKey(a => a.TourPlanId);
        builder.HasMany(x => x.Agendas).WithOne().HasForeignKey(a => a.TourPlanId);
        builder.HasMany(x => x.CostCentres).WithOne().HasForeignKey(a => a.TourPlanId);
        builder.HasMany(x => x.DaBreaks).WithOne().HasForeignKey(a => a.TourPlanId);
        builder.HasMany(x => x.Expenses).WithOne().HasForeignKey(a => a.TourPlanId);
        builder.HasMany(x => x.IntSchedules).WithOne().HasForeignKey(a => a.TourPlanId);
        builder.HasMany(x => x.Leaves).WithOne().HasForeignKey(a => a.TourPlanId);
        builder.HasMany(x => x.NmsSchedules).WithOne().HasForeignKey(a => a.TourPlanId);
        builder.HasMany(x => x.SelfExpenses).WithOne().HasForeignKey(a => a.TourPlanId);

        builder.Ignore(x => x.DomainEvents);
    }
}
