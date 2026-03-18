using FillingOperationService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FillingOperationService.Infrastructure.Persistence.Configurations;

public class PlanDeviationConfiguration : IEntityTypeConfiguration<PlanDeviation>
{
    public void Configure(EntityTypeBuilder<PlanDeviation> builder)
    {
        builder.ToTable("PLAN_DEVIATION");
        builder.HasKey(x => x.ReasonId);
        builder.Property(x => x.ReasonId).HasColumnName("REASON_ID").ValueGeneratedOnAdd();
        builder.Property(x => x.PlanDate).HasColumnName("PLAN_DATE").IsRequired();
        builder.Property(x => x.FillingLineId).HasColumnName("FILLING_LINE_ID").IsRequired();
        builder.Property(x => x.ProductId).HasColumnName("PRODUCT_ID").IsRequired();
        builder.Property(x => x.Reason).HasColumnName("REASON").HasMaxLength(200);

        builder.Ignore(x => x.Id);
        builder.Ignore(x => x.DomainEvents);
    }
}
