using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelRequestService.Domain.Entities;

namespace TravelRequestService.Infrastructure.Data.Configurations;

public class DashTourPlanConfiguration : IEntityTypeConfiguration<DashTourPlan>
{
    public void Configure(EntityTypeBuilder<DashTourPlan> builder)
    {
        builder.ToTable("DASH_TOURPLAN");

        builder.HasNoKey();

        builder.Property(e => e.TourDate).HasColumnName("TOURDATE").HasColumnType("datetime2(3)");
        builder.Property(e => e.Business).HasColumnName("BUSINESS").HasMaxLength(10);
        builder.Property(e => e.Unit).HasColumnName("UNIT").HasMaxLength(20);
        builder.Property(e => e.EmployeeSystemId).HasColumnName("EMPSYSID");
        builder.Property(e => e.EmployeeName).HasColumnName("EMPNAME").HasMaxLength(200);
        builder.Property(e => e.Grade).HasColumnName("GRADE").HasMaxLength(50);
        builder.Property(e => e.GradeCategory).HasColumnName("GRADECATEGORY").HasMaxLength(65);
        builder.Property(e => e.TourNumber).HasColumnName("TOURNO");
        builder.Property(e => e.ExpenseAmount).HasColumnName("EXPAMT").HasColumnType("decimal(19,0)");
        builder.Property(e => e.Nature).HasColumnName("NATURE").HasMaxLength(200);

        builder.Ignore(e => e.DomainEvents);
    }
}
