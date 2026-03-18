using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DevelopmentService.Domain.Entities;

namespace DevelopmentService.Infrastructure.Data.Configurations;

public class LetBhrPlanConfiguration : IEntityTypeConfiguration<LetBhrPlan>
{
    public void Configure(EntityTypeBuilder<LetBhrPlan> builder)
    {
        builder.ToTable("DD_LETBHRPLAN");
        builder.HasKey(x => x.ReqNum);
        builder.Property(x => x.ReqNum).HasColumnName("DD_REQNUM").ValueGeneratedNever();
        builder.Property(x => x.Sno).HasColumnName("DD_SNO");
        builder.Property(x => x.UserId).HasColumnName("DD_USERID").HasMaxLength(255);
        builder.Property(x => x.TrainingProgram).HasColumnName("DD_TRAININGPROGRAM").HasMaxLength(255);
        builder.Property(x => x.TrainingCode).HasColumnName("DD_TRAININGCODE").HasColumnType("decimal(38,0)");
        builder.Property(x => x.Priority).HasColumnName("DD_PRIORITY").HasColumnType("decimal(38,0)");
        builder.Property(x => x.PiNum).HasColumnName("DD_PINUM");
        builder.Property(x => x.FinalAccept).HasColumnName("DD_FINALACCEPT").HasMaxLength(255);
        builder.Property(x => x.BhrAccept).HasColumnName("DD_BHRACCEPT").HasMaxLength(1);

        builder.Ignore(x => x.DomainEvents);
    }
}
