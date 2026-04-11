using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelService.Domain.Entities.TourPlan;

namespace TravelService.Infrastructure.Persistence.Configurations;

public class TourPlanSelfExpenseConfiguration : IEntityTypeConfiguration<TourPlanSelfExpense>
{
    public void Configure(EntityTypeBuilder<TourPlanSelfExpense> builder)
    {
        builder.ToTable("TOURPLAN_SLFEXP");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("EXP_TKTID").HasMaxLength(255);
        builder.Property(x => x.TourPlanId).HasColumnName("EXP_TPID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ExpenseCategory).HasColumnName("EXP_EXPCAT").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TravelMode).HasColumnName("EXP_TRAVELMODE").HasMaxLength(255).IsRequired();
        builder.Property(x => x.FromDate).HasColumnName("EXP_FROMDATE").IsRequired();
        builder.Property(x => x.FromCityId).HasColumnName("EXP_FROMCITY").HasMaxLength(255).IsRequired();
        builder.Property(x => x.FromCityName).HasColumnName("EXP_FROMCITYNAME").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ToDate).HasColumnName("EXP_TODATE").IsRequired();
        builder.Property(x => x.ToCityId).HasColumnName("EXP_TOCITY").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ToCityName).HasColumnName("EXP_TOCITYNAME").HasMaxLength(255).IsRequired();
        builder.Property(x => x.NumberOfDays).HasColumnName("EXP_NOOFDAYS").HasPrecision(18, 4);
        builder.Property(x => x.EntitlementValue).HasColumnName("EXP_ENTITLEVALUE").HasPrecision(18, 4);
        builder.Property(x => x.ExpenseValue).HasColumnName("EXP_VALUE").HasPrecision(18, 4);
        builder.Property(x => x.ServiceTaxValue).HasColumnName("EXP_SERTAXVAL").HasPrecision(18, 4);
        builder.Property(x => x.AdditionalCharges).HasColumnName("EXP_ADLVALUE").HasPrecision(18, 4);
        builder.Property(x => x.TravelClass).HasColumnName("EXP_TRAVELCLASS").HasMaxLength(255).IsRequired();
        builder.Property(x => x.Remarks).HasColumnName("EXP_REMARKS").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ApprovedAmount).HasColumnName("EXP_APPROVEDAMT").HasPrecision(18, 4);
        builder.Property(x => x.FinanceRemarks).HasColumnName("EXP_FINREMARKS").HasMaxLength(255);
        builder.Property(x => x.ExpenseId).HasColumnName("EXP_EXPID").HasMaxLength(255).IsRequired();
        builder.Ignore(x => x.DomainEvents);
    }
}
