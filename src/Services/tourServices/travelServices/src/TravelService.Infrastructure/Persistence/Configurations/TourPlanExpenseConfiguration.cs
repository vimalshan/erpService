using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelService.Domain.Entities.TourPlan;

namespace TravelService.Infrastructure.Persistence.Configurations;

public class TourPlanExpenseConfiguration : IEntityTypeConfiguration<TourPlanExpense>
{
    public void Configure(EntityTypeBuilder<TourPlanExpense> builder)
    {
        builder.ToTable("TOURPLAN_EXPENSE");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("TPEXP_ID").HasMaxLength(255);
        builder.Property(x => x.TourPlanId).HasColumnName("TPEXP_TPID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ExpenseId).HasColumnName("TPEXP_EXPID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.Currency).HasColumnName("TPEXP_CUR").HasMaxLength(255).IsRequired();
        builder.Property(x => x.ExpenseAmount).HasColumnName("TPEXP_EXPAMT").HasPrecision(18, 4);
        builder.Property(x => x.Remarks).HasColumnName("TPEXP_REMARKS").HasMaxLength(255);
        builder.Ignore(x => x.DomainEvents);
    }
}
